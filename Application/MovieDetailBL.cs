using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.services;
using System.Reflection.Metadata.Ecma335;

namespace movie_booking.Application
{
    public class MovieDetailBL
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _dbContext;
        private MovieDetailsService _movieDetailsService;

        public MovieDetailBL(IConfiguration configuration, ApplicationDbContext dbContext, MovieDetailsService movieDetailsService)
        {
            this._dbContext = dbContext;
            this._configuration = configuration;
            this._movieDetailsService = movieDetailsService;
        }

        public async Task<SuccessOrErrorResponseDto<DirectorInfo>> AddDirectorInfo(DirectorInfoDto DirectorInfo)
        {
            if (string.IsNullOrEmpty(DirectorInfo.directName))
            {
                return new SuccessOrErrorResponseDto<DirectorInfo>()
                {
                    StatusCode = 400,
                    Messege = "director info is empty or null",
                    IsSuccess = false,
                };
            }

            var DirectorInfoEntity = new DirectorInfo()
            {
                DirectorName = DirectorInfo.directName,
            };

            try
            {
                this._dbContext.DirectorInfos.Add(DirectorInfoEntity);
                await this._dbContext.SaveChangesAsync();

                return new SuccessOrErrorResponseDto<DirectorInfo>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = "data added successfully",
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<DirectorInfo>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<ActorInfo>> AddActorInfo(ActorInfoDto ActorInfo)
        {
            if (string.IsNullOrEmpty(ActorInfo.ActorName))
            {
                return new SuccessOrErrorResponseDto<ActorInfo>()
                {
                    StatusCode = 400,
                    Messege = "Actor info is empty or null",
                    IsSuccess = false,
                };
            }

            var ActorInfoEntity = new ActorInfo()
            {
                ActorName = ActorInfo.ActorName,
            };

            try
            {
                this._dbContext.ActorInfos.Add(ActorInfoEntity);
                await this._dbContext.SaveChangesAsync();

                return new SuccessOrErrorResponseDto<ActorInfo>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = "data added successfully",
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<ActorInfo>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<WriterInfo>> AddWriterInfo(WriterInfoDto WriterInfo)
        {
            if (string.IsNullOrEmpty(WriterInfo.WriterName))
            {
                return new SuccessOrErrorResponseDto<WriterInfo>()
                {
                    StatusCode = 400,
                    Messege = "Writer info is empty or null",
                    IsSuccess = false,
                };
            }

            var WriterInfoEntity = new WriterInfo()
            {
                WriterName = WriterInfo.WriterName,
            };

            try
            {
                this._dbContext.WriterInfos.Add(WriterInfoEntity);
                await this._dbContext.SaveChangesAsync();

                return new SuccessOrErrorResponseDto<WriterInfo>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = "data added successfully",
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<WriterInfo>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }

        //|| MovieInfo.MovieDurationInfo.Hours is null || !MovieInfo.MovieDurationInfo.Hours.HasValue
        //|| MovieInfo.MovieDurationInfo.Minutes is null || !MovieInfo.MovieDurationInfo.Minutes.HasValue || MovieInfo.MovieDurationInfo.Seconds is null
        //|| !MovieInfo.MovieDurationInfo.Seconds.HasValue || MovieInfo.AppRating is null || !MovieInfo.AppRating.HasValue || MovieInfo.ImdbRating is null
        //|| !MovieInfo.ImdbRating.HasValue

        //|| MovieInfo.MovieTrailer is null || MovieInfo.MovieTrailer.Length == 0

        public async Task<SuccessOrErrorResponseDto<MovieInfo>> AddMovieInfo(MovieInfoDto MovieInfo)
        {

            try
            {

                MovieInfo movieInfo = await this._dbContext.MovieInfos.FirstOrDefaultAsync(mi => mi.MovieName == MovieInfo.MovieName);

                if (movieInfo is null || string.IsNullOrEmpty(movieInfo.MovieName))
                {

                    if (MovieInfo is null || string.IsNullOrEmpty(MovieInfo.Genre) || string.IsNullOrEmpty(MovieInfo.MovieName)
                        || string.IsNullOrEmpty(MovieInfo.WriterName) || string.IsNullOrEmpty(MovieInfo.ActorName)
                        || string.IsNullOrEmpty(MovieInfo.DirectorName) || string.IsNullOrEmpty(MovieInfo.MovieDescription)
                        || string.IsNullOrEmpty(MovieInfo.Audiolanguage) || string.IsNullOrEmpty(MovieInfo.ClassificationAge) || string.IsNullOrEmpty(MovieInfo.SubtitleLanguage)|| MovieInfo.MovieDurationInfo is null || MovieInfo.MovieCover is null || MovieInfo.MovieCover.Length == 0)
                    {

                        return new SuccessOrErrorResponseDto<MovieInfo>
                        {
                            StatusCode = 400,
                            IsSuccess = false,
                            Messege = "one or more feilds is empty / please pass all data",
                        };
                    }

                    MovieInfo MovieInfoAddEntity = new MovieInfo
                    {
                        Genre = MovieInfo.Genre,
                        MovieName = MovieInfo.MovieName,
                        MovieDescription = MovieInfo.MovieDescription,
                        MovieCover = MovieInfo.MovieCover.FileName,
                        //MovieTrailer = MovieInfo.MovieTrailer.FileName,
                        MovieDuration = new TimeSpan(MovieInfo.MovieDurationInfo.Hours, MovieInfo.MovieDurationInfo.Minutes, MovieInfo.MovieDurationInfo.Seconds),
                        ImdbRating = MovieInfo.ImdbRating,
                        AppRating = MovieInfo.AppRating,
                        ClassificationAge = MovieInfo.ClassificationAge,
                        Audiolanguage = MovieInfo.Audiolanguage,
                        SubtitleLanguage = MovieInfo.SubtitleLanguage,
                    };

                    this._dbContext.MovieInfos.Add(MovieInfoAddEntity);
                    await this._dbContext.SaveChangesAsync();
                }

                //if (MovieInfo is null || string.IsNullOrEmpty(MovieInfo.Genre) ||
                //string.IsNullOrEmpty(MovieInfo.MovieName) || string.IsNullOrEmpty(MovieInfo.WriterName)
                //|| string.IsNullOrEmpty(MovieInfo.ActorName) || string.IsNullOrEmpty(MovieInfo.DirectorName)
                //|| string.IsNullOrEmpty(MovieInfo.MovieDescription) || string.IsNullOrEmpty(MovieInfo.Audiolanguage)
                //|| string.IsNullOrEmpty(MovieInfo.ClassificationAge) || string.IsNullOrEmpty(MovieInfo.SubtitleLanguage)
                //|| MovieInfo.MovieDurationInfo is null || MovieInfo.MovieCover is null || MovieInfo.MovieCover.Length == 0)
                //{

                //    return new SuccessOrErrorResponseDto<MovieInfo>
                //    {
                //        StatusCode = 400,
                //        IsSuccess = false,
                //        Messege = "one or more feilds is empty / please pass all data",
                //    };
                //}

                // --- for uploading movie cover images ---
                string uploadPathForMovieCovers = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "MovieCovers");
                if (!Directory.Exists(uploadPathForMovieCovers))
                {
                    Directory.CreateDirectory(uploadPathForMovieCovers);
                }

                var filePathForMovieCovers = Path.Combine(uploadPathForMovieCovers, MovieInfo.MovieCover.FileName);
                using (var stream = System.IO.File.Create(filePathForMovieCovers))
                {
                    await MovieInfo.MovieCover.CopyToAsync(stream);
                }

                // --- for uploading movie trilers ---
                //string uploadPathForMovieTrailers = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "MovieTrailers");
                //if (!Directory.Exists(uploadPathForMovieTrailers))
                //{
                //    Directory.CreateDirectory(uploadPathForMovieTrailers);
                //}

                //var filePathForMovieTrailers = Path.Combine(uploadPathForMovieTrailers, MovieInfo.MovieTrailer.FileName);
                //using (var stream = System.IO.File.Create(filePathForMovieTrailers))
                //{
                //    await MovieInfo.MovieTrailer.CopyToAsync(stream);
                //}

                //service for checking value in object is null or empty
                bool isMovieDurationObjEmpty = this._movieDetailsService.checkMovieDuraitonEmpty(MovieInfo.MovieDurationInfo);


                if (!isMovieDurationObjEmpty || MovieInfo.ImdbRating == null || MovieInfo.AppRating == null || !(MovieInfo.ImdbRating is float) || !(MovieInfo.AppRating is float))
                {
                    return new SuccessOrErrorResponseDto<MovieInfo>
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"the movie duration data is empty please pass neccessary hours/minutes/seconds for duration/please pass correct imbdbrating and app rating",
                    };
                }




                // checking whether the data added correctly and fetching the movie data

                SuccessOrErrorResponseDto<MovieRelationalData> isDirectorWriterActorExists = await this._movieDetailsService.DirectorActorWriterExists(MovieInfo.DirectorName, MovieInfo.ActorName, MovieInfo.WriterName);

                if (!isDirectorWriterActorExists.IsSuccess)
                {
                    return new SuccessOrErrorResponseDto<MovieInfo>
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"{isDirectorWriterActorExists.Messege} and data added successfully to movie info table",
                    };
                }

                //MovieInfo movieInfo = await this._dbContext.MovieInfos.FirstOrDefaultAsync(mi => mi.MovieName == MovieInfo.MovieName);

                //if (movieInfo is null || string.IsNullOrEmpty(movieInfo.MovieName)) {
                //    MovieInfo MovieInfoAddEntity = new MovieInfo
                //    {
                //        Genre = MovieInfo.Genre,
                //        MovieName = MovieInfo.MovieName,
                //        MovieDescription = MovieInfo.MovieDescription,
                //        MovieCover = MovieInfo.MovieCover.FileName,
                //        //MovieTrailer = MovieInfo.MovieTrailer.FileName,
                //        MovieDuration = new TimeSpan(MovieInfo.MovieDurationInfo.Hours, MovieInfo.MovieDurationInfo.Minutes, MovieInfo.MovieDurationInfo.Seconds),
                //        ImdbRating = MovieInfo.ImdbRating,
                //        AppRating = MovieInfo.AppRating,
                //        ClassificationAge = MovieInfo.ClassificationAge,
                //        Audiolanguage = MovieInfo.Audiolanguage,
                //        SubtitleLanguage = MovieInfo.SubtitleLanguage,
                //    };

                //    this._dbContext.MovieInfos.Add(MovieInfoAddEntity);
                //    await this._dbContext.SaveChangesAsync();
                //}

                movieInfo.DirectorInfo.Add(isDirectorWriterActorExists.Data.DirectorData);
                //this._dbContext.MoviesInfoActors.Add(movieInfoActorEntity);
                movieInfo.WriterInfo.Add(isDirectorWriterActorExists.Data.WriterData);
                //this._dbContext.MoviesInfoWriters.Add(movieInfoWriterEntity);
                movieInfo.ActorInfo.Add(isDirectorWriterActorExists.Data.ActorData);
                await this._dbContext.SaveChangesAsync();
                return new SuccessOrErrorResponseDto<MovieInfo>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"data and many to many relationship added",
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<MovieInfo>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }

        }

        public async Task<SuccessOrErrorResponseDto<MovieInfo>> GetMovieDetails(int id)
        {
            try
            {
                //MovieInfo movieDetails = await this._dbContext.MovieInfos
                //    .Include(mi => mi.MovieInfoDirectors)
                //    .Include(mi => mi.MovieInfoWriters)
                //    .Include(mi => mi.MovieInfoActors)
                //    .FirstOrDefaultAsync(mi => mi.Id == id);

                MovieInfo movieDetails = await this._dbContext.MovieInfos
                    .Include(mi => mi.DirectorInfo)
                    .Include(mi => mi.WriterInfo)
                    .Include(mi => mi.ActorInfo)
                    .FirstOrDefaultAsync(mi => mi.Id == id);

                //MovieInfoDirector movieDetails = await this._dbContext.MoviesInfoDirectors
                //    .Include(mid => mid.MovieInfo)
                //    .Include(mid => mid.DirectorInfo)
                //    .FirstOrDefaultAsync(mid => mid.MovieInfoId == id);

                // || movieDetails.MovieInfoDirectors.Count == 0
                // || movieDetails.MovieInfoWriters.Count == 0
                // || movieDetails.MovieInfoActors.Count == 0
                if (movieDetails is null)
                {

                    return new SuccessOrErrorResponseDto<MovieInfo>
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"movie info obj is null or not exists in db/movieinfo directors/movieinfo writers/movieinfo actors is null or empty",
                    };
                }

                return new SuccessOrErrorResponseDto<MovieInfo>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Data = movieDetails,
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<MovieInfo>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
            ;
        }
    }

}

