using CollectionApi.Models.Classes.V1;
using CollectionApi.Models.DTOs.V1;

namespace CollectionApi.Models.Converters.V1
{
    public class CollectionConverter
    {
        public static Collection ConvertCollectionDtoToCollection(CollectionDto source)
        {
            return new Collection()
            {
                Id = source.COLLECTION_ID,
                CollectionName = source.COLLECTION_NAME,
                Description = source.COLLECTION_DESCRIPTION,
                CreatedBy = source.COLLECTION_CREATED_BY,
                CreatedDate = source.COLLECTION_CREATED_DATE,
                LastModifiedBy = source.COLLECTION_LAST_MODIFIED_BY,
                LastModifiedDate = source.COLLECTION_LAST_MODIFIED_DATE
            };
        }

        public static CollectionDto ConvertCollectionToCollectionDto(Collection source)
        {
            return new CollectionDto()
            {
                COLLECTION_ID = source.Id,
                COLLECTION_NAME = source.CollectionName,
                COLLECTION_DESCRIPTION = source.Description,
                COLLECTION_CREATED_BY = source.CreatedBy,
                COLLECTION_CREATED_DATE = source.CreatedDate,
                COLLECTION_LAST_MODIFIED_BY = source.LastModifiedBy,
                COLLECTION_LAST_MODIFIED_DATE = source.LastModifiedDate
            };
        }

        public static List<Collection> ConvertListCollectionDtoToListCollection(List<CollectionDto> source)
        {
            List<Collection> list = new List<Collection>();

            foreach (CollectionDto collectionDto in source)
            {
                Collection collection = new Collection()
                {
                    Id = collectionDto.COLLECTION_ID,
                    CollectionName = collectionDto.COLLECTION_NAME,
                    Description = collectionDto.COLLECTION_DESCRIPTION,
                    CreatedBy = collectionDto.COLLECTION_CREATED_BY,
                    CreatedDate = collectionDto.COLLECTION_CREATED_DATE,
                    LastModifiedBy = collectionDto.COLLECTION_LAST_MODIFIED_BY,
                    LastModifiedDate = collectionDto.COLLECTION_LAST_MODIFIED_DATE
                };

                list.Add(collection);
            }

            return list;
        }

        public static List<CollectionDto> ConvertListCollectionToListCollectionDto(List<Collection> source)
        {
            List<CollectionDto> list = new List<CollectionDto>();

            foreach (Collection collection in source)
            {
                CollectionDto collectionDto = new CollectionDto()
                {
                    COLLECTION_ID = collection.Id,
                    COLLECTION_NAME = collection.CollectionName,
                    COLLECTION_DESCRIPTION = collection.Description,
                    COLLECTION_CREATED_BY = collection.CreatedBy,
                    COLLECTION_CREATED_DATE = collection.CreatedDate,
                    COLLECTION_LAST_MODIFIED_BY = collection.LastModifiedBy,
                    COLLECTION_LAST_MODIFIED_DATE = collection.LastModifiedDate
                };

                list.Add(collectionDto);
            }

            return list;
        }
    }
}
