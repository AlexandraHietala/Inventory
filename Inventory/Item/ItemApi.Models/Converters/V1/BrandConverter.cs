using ItemApi.Models.Classes.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Models.Converters.V1
{
    public class BrandConverter
    {
        public static Brand ConvertBrandDtoToBrand(BrandDto source)
        {
            return new Brand()
            {
                Id = source.BRAND_ID,
                BrandName = source.BRAND_NAME,
                Description = source.BRAND_DESCRIPTION,
                CreatedBy = source.BRAND_CREATED_BY,
                CreatedDate = source.BRAND_CREATED_DATE,
                LastModifiedBy = source.BRAND_LAST_MODIFIED_BY,
                LastModifiedDate = source.BRAND_LAST_MODIFIED_DATE
            };
        }

        public static BrandDto ConvertBrandToBrandDto(Brand source)
        {
            return new BrandDto()
            {
                BRAND_ID = source.Id,
                BRAND_NAME = source.BrandName,
                BRAND_DESCRIPTION = source.Description,
                BRAND_CREATED_BY = source.CreatedBy,
                BRAND_CREATED_DATE = source.CreatedDate,
                BRAND_LAST_MODIFIED_BY = source.LastModifiedBy,
                BRAND_LAST_MODIFIED_DATE = source.LastModifiedDate
            };
        }

        public static List<Brand> ConvertListBrandDtoToListBrand(List<BrandDto> source)
        {
            List<Brand> list = new List<Brand>();

            foreach (BrandDto brandDto in source)
            {
                Brand brand = new Brand()
                {
                    Id = brandDto.BRAND_ID,
                    BrandName = brandDto.BRAND_NAME,
                    Description = brandDto.BRAND_DESCRIPTION,
                    CreatedBy = brandDto.BRAND_CREATED_BY,
                    CreatedDate = brandDto.BRAND_CREATED_DATE,
                    LastModifiedBy = brandDto.BRAND_LAST_MODIFIED_BY,
                    LastModifiedDate = brandDto.BRAND_LAST_MODIFIED_DATE
                };

                list.Add(brand);
            }

            return list;
        }

        public static List<BrandDto> ConvertListBrandToListBrandDto(List<Brand> source)
        {
            List<BrandDto> list = new List<BrandDto>();

            foreach (Brand brand in source)
            {
                BrandDto brandDto = new BrandDto()
                {
                    BRAND_ID = brand.Id,
                    BRAND_NAME = brand.BrandName,
                    BRAND_DESCRIPTION = brand.Description,
                    BRAND_CREATED_BY = brand.CreatedBy,
                    BRAND_CREATED_DATE = brand.CreatedDate,
                    BRAND_LAST_MODIFIED_BY = brand.LastModifiedBy,
                    BRAND_LAST_MODIFIED_DATE = brand.LastModifiedDate
                };

                list.Add(brandDto);
            }

            return list;
        }
    }
}
