using ItemApi.Models.Classes.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Models.Converters.V1
{
    public class ItemCommentConverter
    {
        public static ItemComment ConvertItemCommentDtoToItemComment(ItemCommentDto source)
        {
            return new ItemComment()
            {
                Id = source.COMMENT_ID,
                ItemId = source.ITEM_ID,
                Comment = source.COMMENT,
                CreatedBy = source.COMMENT_CREATED_BY,
                CreatedDate = source.COMMENT_CREATED_DATE,
                LastModifiedBy = source.COMMENT_LAST_MODIFIED_BY,
                LastModifiedDate = source.COMMENT_LAST_MODIFIED_DATE
            };
        }

        public static ItemCommentDto ConvertItemCommentToItemCommentDto(ItemComment source)
        {
            return new ItemCommentDto()
            {
                COMMENT_ID = source.Id,
                ITEM_ID = source.ItemId,
                COMMENT = source.Comment,
                COMMENT_CREATED_BY = source.CreatedBy,
                COMMENT_CREATED_DATE = source.CreatedDate,
                COMMENT_LAST_MODIFIED_BY = source.LastModifiedBy,
                COMMENT_LAST_MODIFIED_DATE = source.LastModifiedDate
            };
        }

        public static List<ItemComment> ConvertListItemCommentDtoToListItemComment(List<ItemCommentDto> source)
        {
            List<ItemComment> list = new List<ItemComment>();

            foreach (ItemCommentDto itemCommentDto in source)
            {
                ItemComment itemComment = new ItemComment()
                {
                    Id = itemCommentDto.COMMENT_ID,
                    ItemId = itemCommentDto.ITEM_ID,
                    Comment = itemCommentDto.COMMENT,
                    CreatedBy = itemCommentDto.COMMENT_CREATED_BY,
                    CreatedDate = itemCommentDto.COMMENT_CREATED_DATE,
                    LastModifiedBy = itemCommentDto.COMMENT_LAST_MODIFIED_BY,
                    LastModifiedDate = itemCommentDto.COMMENT_LAST_MODIFIED_DATE
                };

                list.Add(itemComment);
            }

            return list;
        }

        public static List<ItemCommentDto> ConvertListItemCommentToListItemCommentDto(List<ItemComment> source)
        {
            List<ItemCommentDto> list = new List<ItemCommentDto>();

            foreach (ItemComment itemComment in source)
            {
                ItemCommentDto itemCommentDto = new ItemCommentDto()
                {
                    COMMENT_ID = itemComment.Id,
                    ITEM_ID = itemComment.ItemId,
                    COMMENT = itemComment.Comment,
                    COMMENT_CREATED_BY = itemComment.CreatedBy,
                    COMMENT_CREATED_DATE = itemComment.CreatedDate,
                    COMMENT_LAST_MODIFIED_BY = itemComment.LastModifiedBy,
                    COMMENT_LAST_MODIFIED_DATE = itemComment.LastModifiedDate
                };

                list.Add(itemCommentDto);
            }

            return list;
        }
    }
}
