using UserApi.Models.Classes.V1;
using UserApi.Models.DTOs.V1;

namespace UserApi.Models.Converters.V1
{
    public class RoleConverter
    {
        public static Role ConvertRoleDtoToRole(RoleDto source)
        {
            return new Role()
            {
                Id = source.ROLE_ID,
                Description = source.ROLE_DESCRIPTION
            };
        }

        public static RoleDto ConvertRoleToRoleDto(Role source)
        {
            return new RoleDto()
            {
                ROLE_ID = source.Id,
                ROLE_DESCRIPTION = source.Description
            };
        }

        public static List<Role> ConvertListRoleDtosToListRoles(List<RoleDto> source)
        {
            List<Role> list = new List<Role>();

            foreach (RoleDto roleDto in source)
            {
                Role role = new Role()
                {
                    Id = roleDto.ROLE_ID,
                    Description = roleDto.ROLE_DESCRIPTION
                };

                list.Add(role);
            }

            return list;
        }

        public static List<RoleDto> ConvertListRolesToListRoleDtos(List<Role> source)
        {
            List<RoleDto> list = new List<RoleDto>();

            foreach (Role role in source)
            {
                RoleDto roleDto = new RoleDto()
                {
                    ROLE_ID = role.Id,
                    ROLE_DESCRIPTION = role.Description
                };

                list.Add(roleDto);
            }

            return list;
        }
    }
}
