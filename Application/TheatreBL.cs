using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request.Theatre.FirstLevelUploadDto;
using movie_booking.Dtos.Request.Theatre.FourthLevelUploadDto;
using movie_booking.Dtos.Request.Theatre.SecondLevelUploadDto;
using movie_booking.Dtos.Request.Theatre.ThirdLevelUploadDto;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.Models.Ttheatre;
using System.Runtime.Serialization.Formatters;

namespace movie_booking.Application
{
    public class TheatreBL
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _dbContext;
        public bool isSeatAvailable = true;

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

        public async Task<SuccessOrErrorResponseDto<TheatreSeat>> ThirdLevelRowInfoAdd(ScreenRowsDto screenRowsDto) {
            try
            {
                Screen screenInfo = await _dbContext.Screens.FirstOrDefaultAsync(screen => screen.ScreenName == screenRowsDto.ScreenName);
                TheatreInfo theatreInfo = await _dbContext.TheatreInfos.FirstOrDefaultAsync(theatre => theatre.TheatreTitle == screenRowsDto.TheatreName);

                if (screenInfo is null || theatreInfo is null)
                {
                    return new SuccessOrErrorResponseDto<TheatreSeat>()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"theatre or screeninfo cant be found"
                    };
                }

                var ScreenRowEntity = new ScreenRow()
                {
                    RowName = screenRowsDto.RowName,
                    RowType = screenRowsDto.RowType,
                    RowTicketPrice = screenRowsDto.RowTicketPrice,
                    RowSeatsCount = screenRowsDto.RowSeatsCount,
                    ScreenId = screenInfo?.Id,
                    TheatreId = theatreInfo?.Id
                };
                this._dbContext.ScreenRows.Add(ScreenRowEntity);
                await _dbContext.SaveChangesAsync();

                for (int i = 1; i <= screenRowsDto.RowSeatsCount; i++)
                {
                    var theatreSeat = new TheatreSeat()
                    {
                        SeatNumber = screenRowsDto.RowName + i,
                        //SeatAvailabilityStatus = screenRowsDto.TheatreSeatAvailability,
                        SeatTicketPrice = screenRowsDto.RowTicketPrice,
                        ScreenId = screenInfo.Id,
                    };
                    //if (screenRowsDto?.NotAvailableSeatNumber.Count != 0)
                    //{
                    //    foreach (int notAvailableSeatNumber in screenRowsDto.NotAvailableSeatNumber)
                    //    {
                    //        TheatreSeat notAvailableSeat = 
                    //        theatreSeat = new TheatreSeat()
                    //        {
                    //            SeatAvailabilityStatus = screenRowsDto.IsAnySeatNotAvailable,
                    //        };
                    //    }
                    //}
                    this._dbContext.TheatreSeats.Add(theatreSeat);
                }
                await _dbContext.SaveChangesAsync();
                return new SuccessOrErrorResponseDto<TheatreSeat>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"screen rows and row seats added successfully",
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<TheatreSeat>() {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<ShowsList>> FourthLevelRowInfoAdd(ICollection<ShowListUploadDto> showListUploads) {

            if (showListUploads.Count == 0) {
                return new SuccessOrErrorResponseDto<ShowsList>() {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"showlist array is empty",
                };
            }

            try
            {
                foreach (ShowListUploadDto showList in showListUploads)
                {
                    string[] showDate = showList.Date.Split("-");
                    string[] showStartTime = showList.ShowStartTime.Split(":");
                    string[] showEndTime = showList.ShowEndTime.Split(":");

                    int.TryParse(showDate[0], out int showYear);
                    int.TryParse(showDate[1], out int showMonth);
                    int.TryParse(showDate[2], out int showDay);

                    int.TryParse(showStartTime[0], out int ShowStartHour);
                    int.TryParse(showStartTime[1], out int showStartMinute);

                    int.TryParse(showEndTime[0], out int ShowEndHour);
                    int.TryParse(showEndTime[1], out int showEndMinute);

                    MovieInfo movie = await this._dbContext.MovieInfos.FirstOrDefaultAsync(mi => mi.Id == showList.MovieInfoId);
                    Screen screen = await this._dbContext.Screens.FirstOrDefaultAsync(s => s.Id == showList.ScreenId);
                    if (movie == null || screen == null)
                    {
                        return new SuccessOrErrorResponseDto<ShowsList>()
                        {
                            StatusCode = 400,
                            IsSuccess = false,
                            Messege = $"failed to get movie and screen from database",
                        };
                    }
                    this._dbContext.ShowsLists.Add(new ShowsList()
                    {
                        ShowDate = new DateOnly(showYear, showMonth, showDay),
                        ShowStart = new TimeSpan(ShowStartHour, showStartMinute, 00),
                        ShowEnd = new TimeSpan(ShowEndHour, showEndMinute, 00),
                        MovieInfoId = movie.Id,
                        ScreenId = screen.Id,
                    });
                }
                await this._dbContext.SaveChangesAsync();
                return new SuccessOrErrorResponseDto<ShowsList>() { 
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"Successfully added showList"
                };
            } catch (Exception ex) {
                return new SuccessOrErrorResponseDto<ShowsList>() {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            };
 
        }

    }
}
