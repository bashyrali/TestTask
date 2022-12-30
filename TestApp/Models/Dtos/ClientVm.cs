using System.ComponentModel.DataAnnotations;

namespace TestApp.Models.Dtos
{
    public class ClientVm
    {
        [Required(ErrorMessage = "Поле обязательно для ввода")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "ИИН Должен состоять из цифр")]
        [StringLength(12,MinimumLength = 12,ErrorMessage = "ИИН состоит из 12 символов")]
        public string Iin { get; set; }
        [Required(ErrorMessage = "Поле обязательно для ввода")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public string Email { get; set; }

        public bool IsAvailable { get; set; }
    }
}