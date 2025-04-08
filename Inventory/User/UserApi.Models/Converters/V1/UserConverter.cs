using UserApi.Models.Classes.V1;
using UserApi.Models.DTOs.V1;

namespace UserApi.Models.Converters.V1
{
    public class UserConverter
    {
        public static User ConvertUserDtoToUser(UserDto source)
        {
            return new User()
            {
                Id = source.ID,
                Name = source.NAME,
                PassSalt = source.PASS_SALT,
                PassHash = source.PASS_HASH,
                RoleId = (source.ROLE_ID != null ? source.ROLE_ID : null),
                CreatedBy = source.CREATED_BY,
                CreatedDate = source.CREATED_DATE,
                LastModifiedBy = source.LAST_MODIFIED_BY,
                LastModifiedDate = source.LAST_MODIFIED_DATE
            };
        }

        public static UserDto ConvertUserToUserDto(User source)
        {
            return new UserDto()
            {
                ID = source.Id,
                NAME = source.Name,
                PASS_SALT = source.PassSalt,
                PASS_HASH = source.PassHash,
                ROLE_ID = (source.RoleId != null ? source.RoleId : null),
                CREATED_BY = source.CreatedBy,
                CREATED_DATE = source.CreatedDate,
                LAST_MODIFIED_BY = source.LastModifiedBy,
                LAST_MODIFIED_DATE = source.LastModifiedDate
            };
        }

        public static List<User> ConvertListUserDtoToListUser(List<UserDto> source)
        {
            List<User> list = new List<User>();

            foreach (UserDto userDto in source)
            {
                User user = new User()
                {
                    Id = userDto.ID,
                    Name = userDto.NAME,
                    PassSalt = userDto.PASS_SALT,
                    PassHash = userDto.PASS_HASH,
                    RoleId = (userDto.ROLE_ID != null ? userDto.ROLE_ID : null),
                    CreatedBy = userDto.CREATED_BY,
                    CreatedDate = userDto.CREATED_DATE,
                    LastModifiedBy = userDto.LAST_MODIFIED_BY,
                    LastModifiedDate = userDto.LAST_MODIFIED_DATE
                };

                list.Add(user);
            }

            return list;
        }

        public static List<UserDto> ConvertListUserToListUserDto(List<User> source)
        {
            List<UserDto> list = new List<UserDto>();

            foreach (User user in source)
            {
                UserDto userDto = new UserDto()
                {
                    ID = user.Id,
                    NAME = user.Name,
                    PASS_SALT = user.PassSalt,
                    PASS_HASH = user.PassHash,
                    ROLE_ID = (user.RoleId != null ? user.RoleId : null),
                    CREATED_BY = user.CreatedBy,
                    CREATED_DATE = user.CreatedDate,
                    LAST_MODIFIED_BY = user.LastModifiedBy,
                    LAST_MODIFIED_DATE = user.LastModifiedDate
                };

                list.Add(userDto);
            }

            return list;
        }


    }
}
