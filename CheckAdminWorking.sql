select AspNetUsers.StudentID, AspNetUsers.Email, AspNetRoles.Name
from AspNetUsers, AspNetRoles, AspNetUserRoles
where AspNetUsers.id = AspNetUserRoles.UserId and
AspNetRoles.Id= AspNetUserRoles.RoleId