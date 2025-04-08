using ItemApi.Models.Classes.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Models.Converters.V1
{
    public class SeriesConverter
    {
        public static Series ConvertSeriesDtoToSeries(SeriesDto source)
        {
            return new Series()
            {
                Id = source.SERIES_ID,
                SeriesName = source.SERIES_NAME,
                Description = source.SERIES_DESCRIPTION,
                CreatedBy = source.SERIES_CREATED_BY,
                CreatedDate = source.SERIES_CREATED_DATE,
                LastModifiedBy = source.SERIES_LAST_MODIFIED_BY,
                LastModifiedDate = source.SERIES_LAST_MODIFIED_DATE
            };
        }

        public static SeriesDto ConvertSeriesToSeriesDto(Series source)
        {
            return new SeriesDto()
            {
                SERIES_ID = source.Id,
                SERIES_NAME = source.SeriesName,
                SERIES_DESCRIPTION = source.Description,
                SERIES_CREATED_BY = source.CreatedBy,
                SERIES_CREATED_DATE = source.CreatedDate,
                SERIES_LAST_MODIFIED_BY = source.LastModifiedBy,
                SERIES_LAST_MODIFIED_DATE = source.LastModifiedDate
            };
        }

        public static List<Series> ConvertListSeriesDtoToListSeries(List<SeriesDto> source)
        {
            List<Series> list = new List<Series>();

            foreach (SeriesDto seriesDto in source)
            {
                Series series = new Series()
                {
                    Id = seriesDto.SERIES_ID,
                    SeriesName = seriesDto.SERIES_NAME,
                    Description = seriesDto.SERIES_DESCRIPTION,
                    CreatedBy = seriesDto.SERIES_CREATED_BY,
                    CreatedDate = seriesDto.SERIES_CREATED_DATE,
                    LastModifiedBy = seriesDto.SERIES_LAST_MODIFIED_BY,
                    LastModifiedDate = seriesDto.SERIES_LAST_MODIFIED_DATE
                };

                list.Add(series);
            }

            return list;
        }

        public static List<SeriesDto> ConvertListSeriesToListSeriesDto(List<Series> source)
        {
            List<SeriesDto> list = new List<SeriesDto>();

            foreach (Series series in source)
            {
                SeriesDto seriesDto = new SeriesDto()
                {
                    SERIES_ID = series.Id,
                    SERIES_NAME = series.SeriesName,
                    SERIES_DESCRIPTION = series.Description,
                    SERIES_CREATED_BY = series.CreatedBy,
                    SERIES_CREATED_DATE = series.CreatedDate,
                    SERIES_LAST_MODIFIED_BY = series.LastModifiedBy,
                    SERIES_LAST_MODIFIED_DATE = series.LastModifiedDate
                };

                list.Add(seriesDto);
            }

            return list;
        }
    }
}
