using ItemApi.Models.Classes.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Models.Converters.V1
{
    public class ItemConverter
    {
        public static Item ConvertItemDtoToItem(ItemDto source)
        {
            return new Item()
            {
                Id = source.ID,
                Status = source.STATUS,
                Type = source.TYPE,
                Brand = source.BRAND,
                Series = source.SERIES,
                Name = source.NAME,
                Description = source.DESCRIPTION,
                Format = source.FORMAT,
                Size = source.SIZE,
                Year = source.YEAR,
                Photo = source.PHOTO,
                CreatedBy = source.CREATED_BY,
                CreatedDate = source.CREATED_DATE,
                LastModifiedBy = source.LAST_MODIFIED_BY,
                LastModifiedDate = source.LAST_MODIFIED_DATE
            };
        }

        public static ItemDto ConvertItemToItemDto(Item source)
        {
            return new ItemDto()
            {
                ID = source.Id,
                STATUS = source.Status,
                TYPE = source.Type,
                BRAND = source.Brand,
                SERIES = source.Series,
                NAME = source.Name,
                DESCRIPTION = source.Description,
                FORMAT = source.Format,
                SIZE = source.Size,
                YEAR = source.Year,
                PHOTO = source.Photo,
                CREATED_BY = source.CreatedBy,
                CREATED_DATE = source.CreatedDate,
                LAST_MODIFIED_BY = source.LastModifiedBy,
                LAST_MODIFIED_DATE = source.LastModifiedDate
            };
        }

        public static List<Item> ConvertListItemDtoToListItem(List<ItemDto> source)
        {
            List<Item> list = new List<Item>();

            foreach (ItemDto itemDto in source)
            {
                Item item = new Item()
                {
                    Id = itemDto.ID,
                    Status = itemDto.STATUS,
                    Type = itemDto.TYPE,
                    Brand = itemDto.BRAND,
                    Series = itemDto.SERIES,
                    Name = itemDto.NAME,
                    Description = itemDto.DESCRIPTION,
                    Format = itemDto.FORMAT,
                    Size = itemDto.SIZE,
                    Year = itemDto.YEAR,
                    Photo = itemDto.PHOTO,
                    CreatedBy = itemDto.CREATED_BY,
                    CreatedDate = itemDto.CREATED_DATE,
                    LastModifiedBy = itemDto.LAST_MODIFIED_BY,
                    LastModifiedDate = itemDto.LAST_MODIFIED_DATE
                };

                list.Add(item);
            }

            return list;
        }

        public static List<ItemDto> ConvertListItemToListItemDto(List<Item> source)
        {
            List<ItemDto> list = new List<ItemDto>();

            foreach (Item item in source)
            {
                ItemDto itemDto = new ItemDto()
                {
                    ID = item.Id,
                    STATUS = item.Status,
                    TYPE = item.Type,
                    BRAND = item.Brand,
                    SERIES = item.Series,
                    NAME = item.Name,
                    DESCRIPTION = item.Description,
                    FORMAT = item.Format,
                    SIZE = item.Size,
                    YEAR = item.Year,
                    PHOTO = item.Photo,
                    CREATED_BY = item.CreatedBy,
                    CREATED_DATE = item.CreatedDate,
                    LAST_MODIFIED_BY = item.LastModifiedBy,
                    LAST_MODIFIED_DATE = item.LastModifiedDate
                };

                list.Add(itemDto);
            }

            return list;
        }
    }
}
