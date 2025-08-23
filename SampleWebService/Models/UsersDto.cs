using System.ComponentModel;

namespace SampleWebService.Models
{
    public class UsersDto
    {
        public int Id { get; set; }

        [FieldMetadata("Ad", "text", true)]
        public string Name { get; set; }

        [FieldMetadata("Soyad", "text", true)]
        public string Surname { get; set; }

        [FieldMetadata("Şifre", "password", true)]
        public string Password { get; set; }

        [FieldMetadata("Telefon", "tel")]
        public string Phone { get; set; }

        [FieldMetadata("Kullanıcı Adı", "text", true)]
        public string Username { get; set; }

        [FieldMetadata("Grup Id", "select", true, "https://localhost:7116/usergroups", upperItem: "userGroupName", onlyTable: true)]
        public int UserGroupId { get; set; }

        [FieldMetadata("Grup Adı", "text")]
        public string UserGroupName { get; set; }
    }
}
