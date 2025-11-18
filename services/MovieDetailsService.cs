using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using System.IO;

namespace movie_booking.services
{
    public class MovieDetailsService
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _dbContext;
        public MovieDetailsService(IConfiguration _configuration, ApplicationDbContext _dbContext) {
            this._configuration = _configuration;
            this._dbContext = _dbContext;
        }

        public bool checkMovieDuraitonEmpty(MovieDurationInfo movieDurationInfo) {
            foreach (var movieDurationvalue in movieDurationInfo.GetType().GetProperties()) {
                if (!(movieDurationvalue is null))
                { // (movieDurationvalue is int) &&   
                    return true;
                }
                return false;
            }
            return false;
        }


        public async Task<SuccessOrErrorResponseDto<DirectorInfo>> directorExists(string DirectorName) {
            try
            {
                DirectorInfo director = await this._dbContext.DirectorInfos.FirstOrDefaultAsync(d => d.DirectorName == DirectorName);
                if (!(director == null) || !(string.IsNullOrEmpty(director.DirectorName)))
                {
                    return new SuccessOrErrorResponseDto<DirectorInfo> { 
                        StatusCode = 200,
                        IsSuccess = true,
                        Messege = $"director data fetched successfully",
                        Data = director,
                    };
                }
                return new SuccessOrErrorResponseDto<DirectorInfo>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"director does not exists or please pass correct director name",
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<DirectorInfo> { 
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }


        public async Task<SuccessOrErrorResponseDto<WriterInfo>> WriterExists(string WriterName)
        {
            try
            {
                WriterInfo writer = await this._dbContext.WriterInfos.FirstOrDefaultAsync(w => w.WriterName == WriterName);
                if (!(writer == null) || !(string.IsNullOrEmpty(writer.WriterName)))
                {
                    return new SuccessOrErrorResponseDto<WriterInfo>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Messege = $"writer data fetched successfully",
                        Data = writer,
                    }; 
                }
                return new SuccessOrErrorResponseDto<WriterInfo>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"writer does not exists or please pass correct writer name",
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<WriterInfo>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }


        public async Task<SuccessOrErrorResponseDto<ActorInfo>> ActorExists(string WriterName)
        {
            try
            {
                ActorInfo actor = await this._dbContext.ActorInfos.FirstOrDefaultAsync(a => a.ActorName == WriterName);
                if (!(actor == null) || !(string.IsNullOrEmpty(actor.ActorName)))
                {
                    return new SuccessOrErrorResponseDto<ActorInfo>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Messege = $"actor data fetched successfully",
                        Data = actor,
                    }; ;
                }
                return new SuccessOrErrorResponseDto<ActorInfo>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"Actor does not exists or please pass correct writer name",
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<ActorInfo>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }


        public async Task<SuccessOrErrorResponseDto<MovieRelationalData>> DirectorActorWriterExists(string DirectorName, string ActorName, string WriterName) {
            
            SuccessOrErrorResponseDto<DirectorInfo> DirectorExists = await this.directorExists(DirectorName);
            SuccessOrErrorResponseDto<WriterInfo> WriterExists = await this.WriterExists(WriterName);
            SuccessOrErrorResponseDto<ActorInfo> ActorExists = await this.ActorExists(ActorName);

            if (DirectorExists.IsSuccess && ActorExists.IsSuccess && WriterExists.IsSuccess) {
                return new SuccessOrErrorResponseDto<MovieRelationalData>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"{DirectorExists.Messege} {ActorExists.Messege} {WriterExists.Messege}",
                    Data = new MovieRelationalData
                    {
                        DirectorData = DirectorExists.Data,
                        WriterData = WriterExists.Data,
                        ActorData = ActorExists.Data,
                    }
                };
            }

            return new SuccessOrErrorResponseDto<MovieRelationalData>
            {
                StatusCode = 400,
                IsSuccess = false,
                Messege = $"{DirectorExists.Messege} {ActorExists.Messege} {WriterExists.Messege}",
            };
        }

    }
}
