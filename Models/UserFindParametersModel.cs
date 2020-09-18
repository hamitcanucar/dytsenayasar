using System;
using dytsenayasar.DataAccess.Entities;

namespace dyt_ecommerce.Models
{
    public class UserFindParametersModel
    {
        public string SearchValue { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public GenderType? Gender { get; set; }
        public UserType? UserType { get; set; }
    }
}