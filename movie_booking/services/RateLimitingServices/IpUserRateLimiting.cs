using System.Threading.RateLimiting;

namespace movie_booking.services.RateLimitingServices
{
    // should be inherited from the base RateLimit class to call RateLimit.AcquireAsync() and then our custom overided AcquireCoreAsync
    public class IpUserRateLimiting : RateLimiter
    {
        private readonly string _ip;
        private readonly string _userId;
        private readonly PartitionedRateLimiter<string> _ipLimiter;
        private readonly PartitionedRateLimiter<string> _userLimiter;
        public IpUserRateLimiting(string ip, string userId, PartitionedRateLimiter<string> ipLimiter, PartitionedRateLimiter<string> userLimiter) {
            this._ip = ip;
            this._userId = userId;
            this._ipLimiter = ipLimiter;
            this._userLimiter = userLimiter;
        }
        
        public override TimeSpan? IdleDuration
        {
            get
            {
                return null;
            }
        }

        // the acquireAsyncCore method is called after asp.net core builtin AcquireAsync
        protected override async ValueTask<RateLimitLease> AcquireAsyncCore(int permitCount, CancellationToken cancellationToken) {

            // first check if the ip ratelimiter has the permit can be acquired based on the permit count, so the booleon result can be stored in a lease 
            var ipLease = await this._ipLimiter.AcquireAsync(this._ip, 1, cancellationToken);

            // we need to check if the current request users ip is acquired or ratelimter accepts the ip
            if (!ipLease.IsAcquired) {
                // if ip is rejected ie the limit reaches. the execution stops and returns the iplease
                return ipLease;
            }
            // if ip rate limiter check passes then its time to check user limit acquire
            var userLease = await this._userLimiter.AcquireAsync(this._userId, permitCount, cancellationToken);

            // check for the userLease
            if (!userLease.IsAcquired) {
                // dispose the used iplease
                return userLease;
            }

            return new CombinedLease(ipLease, userLease);

        }

        // it is an synchronous acquisition ie it immedialely checks for the permits
        // availble. if the permit limit is 0 it does not wait for the queue but AcquireAsync waits for the queue request to restore right?
        //ie
        //    No permits available, queue full - The request is rejected.
        //    No permits available, QueueLimit = 0 The request is rejected immediately.
        protected override RateLimitLease AttemptAcquireCore(int permitCount)
        {
            throw new NotSupportedException(
                "Use AcquireAsync.");
        }

        public override RateLimiterStatistics? GetStatistics() {
            throw new NotImplementedException();
        }

    }



    public class CombinedLease : RateLimitLease
    { 
        private readonly RateLimitLease _ipLease;
        private readonly RateLimitLease _userLease;
        public CombinedLease(RateLimitLease ipLease, RateLimitLease userLease) { 
            this._ipLease = ipLease;
            this._userLease = userLease;
        }

        // we overide the isAcquired method cause to return both checks passes ie iplimitlease and userlimitlease
        public override bool IsAcquired => this._ipLease.IsAcquired && this._userLease.IsAcquired;

        public override bool TryGetMetadata(
            string metadataName,
            out object? metadata)
        {
            metadata = null;
            return false;
        }

        public override IEnumerable<string> MetadataNames => throw new NotImplementedException();

        // when the ratelimiting middleware is done with the ip and user ratelimit leases we need to dispoonse both the leases
        protected override void Dispose(bool disposing) {
            if (disposing)
            {
                _ipLease.Dispose();
                _userLease.Dispose();
            }

            base.Dispose(disposing);
        }

    }

}
