using movie_booking.data;
using movie_booking.Dtos.Request.Theatre.FirstLevelUploadDto;
using movie_booking.Dtos.Request.Theatre.SecondLevelUploadDto;
using movie_booking.Dtos.Response;
using movie_booking.Models.Ttheatre;

namespace movie_booking.Application
{
    public class TheatreBL
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _dbContext;

        public TheatreBL(IConfiguration Configuration, ApplicationDbContext DbContext) { 
            this._configuration = Configuration;
            this._dbContext = DbContext;
        }

        //theatreinfo and screen onboard
        public async Task<SuccessOrErrorResponseDto<TheatreInfo>> FirstLevelTheatreOnBoard(FirstLevelTheaterInfoDto FirstLevelTheaterInfoDto) {
            if (FirstLevelTheaterInfoDto is null || string.IsNullOrEmpty(FirstLevelTheaterInfoDto.TheatreTitle)
                || FirstLevelTheaterInfoDto.Screen is null
                ||  FirstLevelTheaterInfoDto.Screen.Count == 0) {
                return new SuccessOrErrorResponseDto<TheatreInfo>()
                {
                    StatusCode = 400,
                    Messege = $"theatre title is empty pass neccessary theatre title || screen info array is null or empty array",
                    IsSuccess = false,
                };
            }
            try
            {
                var TheatreOnboardEntity = new TheatreInfo()
                {
                    TheatreTitle = FirstLevelTheaterInfoDto.TheatreTitle,
                };
                this._dbContext.TheatreInfos.Add(TheatreOnboardEntity);
                foreach (FirstLevelScreenInfo firstLevelScreenInfo in FirstLevelTheaterInfoDto.Screen)
                {
                    if (string.IsNullOrEmpty(firstLevelScreenInfo.ScreenName) || string.IsNullOrEmpty(firstLevelScreenInfo.Dimension)
                        || string.IsNullOrEmpty(firstLevelScreenInfo.ProjectionFormat) || string.IsNullOrEmpty(firstLevelScreenInfo.ScreenType)
                        || string.IsNullOrEmpty(firstLevelScreenInfo.AudioSpecific) || string.IsNullOrEmpty(firstLevelScreenInfo.ScreenAspectRatio)
                        || string.IsNullOrEmpty(firstLevelScreenInfo.ScreenType) || string.IsNullOrEmpty(firstLevelScreenInfo.ProjectionFormat) || firstLevelScreenInfo.ScreenCapacity == null || firstLevelScreenInfo.ScreenCapacity == 0)
                    {

                        return new SuccessOrErrorResponseDto<TheatreInfo>()
                        {
                            StatusCode = 400,
                            Messege = $"please enter/pass all of the fields / or screen capacity may may be null or still 0",
                            IsSuccess = false,
                        };
                    }
                    var ScreenOnboardEntity = new Screen()
                    {
                        ScreenName = firstLevelScreenInfo.ScreenName,
                        ScreenCapacity = firstLevelScreenInfo.ScreenCapacity,
                        ScreenType = firstLevelScreenInfo.ScreenType,
                        ProjectionFormat = firstLevelScreenInfo.ProjectionFormat,
                        AspectRatio = firstLevelScreenInfo.ScreenAspectRatio,
                        Dimension = firstLevelScreenInfo.Dimension,
                        Audio = firstLevelScreenInfo.AudioSpecific,
                        IsAirConditioner = firstLevelScreenInfo.IsAcSuppported,
                    };
                    
                    this._dbContext.Screens.Add(ScreenOnboardEntity);
                }
                await this._dbContext.SaveChangesAsync();
                return new SuccessOrErrorResponseDto<TheatreInfo>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"theatre adnd screen onboarded successfully",
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<TheatreInfo>()
                {
                    StatusCode = 500,
                    IsSuccess = true,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<TheatreLocation>> SecondLevelTheatreOnBoard(TheatreLocationDto SecondLevelTheaterInfoDto) {
            if (string.IsNullOrEmpty(SecondLevelTheaterInfoDto.City) || string.IsNullOrEmpty(SecondLevelTheaterInfoDto.State) 
                || string.IsNullOrEmpty(SecondLevelTheaterInfoDto.CountryName)
                || string.IsNullOrEmpty(SecondLevelTheaterInfoDto.CountryCode)
                || string.IsNullOrEmpty(SecondLevelTheaterInfoDto.PostalCode)
                || string.IsNullOrEmpty(SecondLevelTheaterInfoDto.Latitude) 
                || string.IsNullOrEmpty(SecondLevelTheaterInfoDto.Longitude)) {

                return new SuccessOrErrorResponseDto<TheatreLocation>()
                {
                    StatusCode = 400,
                    Messege = $"please enter all location values or some of the location values may be null or empty"
                };
            }
            try
            {
                var TheatreLocationEntity = new TheatreLocation()
                {
                    City = SecondLevelTheaterInfoDto.City,
                    State = SecondLevelTheaterInfoDto.State,
                    CountryName = SecondLevelTheaterInfoDto.CountryName,
                    CountryCode = SecondLevelTheaterInfoDto.CountryCode,
                    PostalCode = SecondLevelTheaterInfoDto.PostalCode,
                    Latitude = SecondLevelTheaterInfoDto.Latitude,
                    Longitude = SecondLevelTheaterInfoDto.Longitude,
                };
                this._dbContext.TheatreLocations.Add(TheatreLocationEntity);
                await this._dbContext.SaveChangesAsync();
                return new SuccessOrErrorResponseDto<TheatreLocation>()
                {
                    StatusCode = 200,
                    Messege = $"Theatre Location Info onbooarded successfully",
                    IsSuccess = true,
                };
            }
            catch(Exception ex) {
                return new SuccessOrErrorResponseDto<TheatreLocation>()
                {
                    StatusCode = 500,
                    Messege = ex.Message,
                    IsSuccess = false,
                };
            }
        }
    }
}
