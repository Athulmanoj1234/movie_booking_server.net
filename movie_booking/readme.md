Rate Limiting 

  It is a technique to limits the quantity or frequency of client requests to prevent overload, maintain stability, and ensure fair resource distribution.
        -> Reduces the risk of resource abuse and denial-of-service (DoS) attacks, improving performance, reliability, and security.
        -> It is used in web servers, APIs, network traffic management, and database access.

   It can be used in the different components of the server
        -> API Rate Limiting :- APIs commonly employ rate limitation to control the volume of client requests, ensure fair access to resources, and prevent abuse.
        -> Web Server Rate Limiting :- Web servers employ rate limitation as a defense against denial-of-service attacks and to prevent server overload.
        -> Database Rate limitation :- To keep the database server from experiencing undue strain and to preserve database performance, rate limitation is applied to database queries. For instance, to avoid resource exhaustion and guarantee seamless functioning, an e-commerce website can restrict the quantity of database queries per user.
        Login Rate restriction:- To stop password guessing and brute-force assaults, login systems employ rate restriction. Systems can prevent unwanted access by restricting the quantity of login attempts made by each person or IP address.


    Different types of rate limiting
        -> IP-based Rate Limiting :- it limits number of web or api requests from a single ip address within a set time period.
            * Even though the data sent back is identical, the server still has to do heavy lifting for every single request:
            * If a server can only handle 1,000 requests per second, and a single bot dumps 50,000 requests onto it in one second, the server's CPU and memory hit 100%. The server crashes, and real, legitimate users can no longer access the website. This is a Denial of Service (DoS) attack.
            * over exploitation of data or datascraping is limited.
            Request 1: ://mysite.com (Returns Product A)Request 2: ://mysite.com (Returns Product B)Request 3: ://mysite.com (Returns Product C)...Request 50,000: ://mysite.com (Returns Product Z)

            Advantages
                -> This approach is widely used due to its simplicity and effectiveness in basic traffic control.
                -> simple to implement at both network and application levels
                -> Helps block excessive traffic from a single source
            Disadvantages
                -> Can be bypassed using VPNs, proxies, or botnets
                -> (very important)May block legitimate users sharing the same IP (e.g., corporate networks)

        
        -> server-based rate limiting :- It limits number of requests a server can handle within a specific timeperiod to prevent overload and maintain performance.
            for eg - Example: A music streaming service allows only 100 requests per second per server to ensure the system remains fast and responsive during peak usage.

            Advantages
                -> This approach helps maintain system stability by controlling traffic at the server level.
                -> Protects servers from being overwhelmed during high traffic
                -> Ensures fair resource usage so no single user degrades performance
            Disadvantages
                -> However, it may not be fully effective in distributed environments.
                -> Can be bypassed if requests are spread across multiple servers
                -> Legitimate users may face delays if limits are too strict or traffic is high

        -> Geographical ratelimiting :- more to be explored......

        Working
            The number of queries a user or system can make to a service in a predetermined period of time can be managed by rate limitation. A service might permit 100 requests per minute, for instance. Any additional requests will be blocked or slowed down by the system until the time window is reset once that limit is reached.


    Rate Limiting algorithms-
       1. Fixed window rate limiting - it specifies how much requests in an specified window can be get handled 
           ie if window expiry is 10 seconds and its limit is 10 so in ten seconds ten requests can be get handled

           eg for registration with options

           -  
           builder.Services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                    {
                        options.PermitLimit = 10;  // requests limit in one window
                        options.Window = TimeSpan.FromSeconds(10); // window expiry ie for one window how much time is the expiry 
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;  // the requests that was in the queue processing order ie the the requests comes first in the queue process first and last request process last
                        options.QueueLimit = 5;  //when the limit is reached how much requests are allowed to wait in the queue
                    });
            });
        
        2. slidiing window rate limiting - the window is divided into segements based on the segements per window and the older segments requests will be flus
        
            builder.Services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
                {
                    options.PermitLimit = 10;
                    options.Window = TimeSpan.FromSeconds(10);
                    options.SegmentsPerWindow = 2;  //says to how much segments should the window to be gets divided.
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 5;
                });
            });



        3. Token Bucket rate limiting - in token bucket rate limiting the rate limiting is done based on the tokens in the bucket



        2. How the sliding window is structured
Imagine the timer boundaries align at 0, 30, 60, 90, and 120 seconds.
The timeline of segments
Segment 1
0–30s

Segment 2
30–60s


Active window: 0–60 seconds

Segment 2
30–60s

Segment 3
60–90s


Active window: 30–90 seconds

Segment 3
60–90s

Segment 4
90–120s


Active window: 60–120 seconds

The window advances by one segment every 30 seconds in this illustration. The oldest segment is removed from the active window, and the segment following it becomes part of the window.
This is not four independent buckets, each with two permits. The limit of two applies across the active window.
3. Full request execution flow
Let's trace what happens when requests arrive at your API.
Assume the limiter's segment boundaries align at 0, 30, 60, 90 seconds, and so on. The exact timing depends on the timer's initialization.
Step 1

A request arrives
The middleware attempts to acquire a permit from the limiter.



Step 2

The limiter checks available permits
If a permit is available, it is consumed and the request is allowed to proceed. Otherwise, the limiter checks the queue configuration.



Step 3

The request is queued or rejected
With QueueLimit = 5, up to five pending acquisitions can wait. If the queue is full, another acquisition is rejected.



Step 4

The automatic timer advances the segments
As the window moves, usage in the expired segment is recycled, returning those permits to the available capacity.



Step 5

A queued acquisition may be granted a permit
The oldest waiting acquisition is prioritized. If it receives a permit, its awaiting operation completes and the request can continue through the middleware pipeline.



Microsoft describes the sliding-window algorithm as recycling requests from the expired segment one window back into the available permit count. 

Microsoft Learn



4. Detailed timeline: two requests arrive near the end of a segment
Let's return to your example:
- Request A arrives at 59.0s.
- Request B arrives at 59.5s.
- Both consume the two available permits.
- Request C arrives at 59.8s and waits in the queue.
The segment containing A and B is the 30–60s segment.
Time	Segment event	A and B's usage	Can C proceed?
59–60s	Segment 2 is active	Both permits consumed	No; it waits
60s	Segment 3 begins	Segment 2's usage remains counted	No
90s	Segment 4 begins	Segment 2's usage is recycled in this illustrative alignment	Yes, if a permit is available and C is next in the queue
Notice the subtlety: A and B's usage can be recycled at the next relevant segment advancement, rather than exactly 60 seconds after each request arrived.
Important clarification: The segmented sliding-window algorithm is an approximation, not an exact per-request rolling 60-second timer. With only two segments, expiration is relatively coarse. A request near the end of a segment can stop counting much sooner than 60 seconds after it arrived. Increasing SegmentsPerWindow makes the approximation finer, at the cost of additional bookkeeping. 

Microsoft Learn



5. How the queue works
Your queue settings are:
QueueLimit = 5,
QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
AutoReplenishment = true


Suppose both permits are consumed and three requests arrive:
Request A → Acquires permit
Request B → Acquires permit

Request C → Waits in queue (1)
Request D → Waits in queue (2)
Request E → Waits in queue (3)


When capacity becomes available, the oldest queued acquisition is prioritized:
A segment expires
       |
       v
Capacity becomes available
       |
       v
Request C receives a permit
       |
       v
Request C continues through the pipeline


The queue holds pending permit acquisitions, not extra permits. If capacity becomes available for only one request, the other queued requests must continue waiting.
Also, AutoReplenishment = true does not itself create new permits every 30 seconds. It enables the timer that advances the sliding-window segments; the amount of capacity freed depends on the usage recorded in the expired segment.
6. What happens when the queue is full?
Suppose the limit is reached and five acquisitions are already waiting.
Permit limit reached
       |
       v
Queue has 5 waiting acquisitions
       |
       v
Another request arrives
       |
       v
Queue is full
       |
       v
Acquisition is rejected


In ASP.NET Core, a rejected rate-limit acquisition can result in a 429 Too Many Requests response, depending on your middleware configuration. 

Microsoft Learn



7. Fixed Window vs Sliding Window
Behavior	Fixed Window	Sliding Window
How time is tracked	One fixed-duration counter	Multiple segments that advance over time
When usage is cleared	All usage at the fixed-window boundary	Usage in an expired segment is recycled
Boundary bursts	Can allow a burst across consecutive windows	Reduces boundary bursts by retaining usage across segments
Queue support	Yes, when configured	Yes, when configured
Exact per-request expiration	No	No; segment-based approximation
For example, with Fixed Window, two requests at 59s can consume both permits, then the counter resets at 60s, allowing two more requests immediately. With Sliding Window, whether those two requests still count at 60s depends on the segment in which their usage was recorded and whether that segment has expired.
8. Applying this to your IP-and-user rate limiter
In your custom IpUserRateLimiting implementation, the flow has an additional layer:
Incoming HTTP request
        |
        v
Identify IP address and admin ID
        |
        v
Find the IP + admin coordinator partition
        |
        v
Acquire from the IP sliding-window limiter
        |
        +---- Rejected/queued? Handle that result
        |
        v
Acquire from the user sliding-window limiter
        |
        +---- Rejected? Dispose the IP lease
        |
        v
Return combined lease
        |
        v
Middleware allows request to continue


One important caveat: disposing a lease from a time-based limiter does not refund a permit that has already been consumed. So if the IP limiter grants a permit but the user limiter rejects, that IP usage can still count until its segment expires. 

Microsoft Learn



9. Final summary for your documentation
The ASP.NET Core sliding-window rate limiter divides a configured time window into smaller segments. Each incoming request must acquire a permit. If no permit is available, its acquisition may wait in a queue or be rejected, depending on the queue configuration. With automatic replenishment enabled, the limiter advances its segments periodically and recycles usage from expired segments, making capacity available to subsequent or queued acquisitions. Unlike Fixed Window, it does not reset the entire counter at one fixed boundary; however, its segment-based expiration is approximate rather than an exact timer for each request.