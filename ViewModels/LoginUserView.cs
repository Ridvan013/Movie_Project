using System.ComponentModel.DataAnnotations;

namespace Movie_Project.ViewModels;

public class LoginUserView{
	[Required]
	public string? Username { get; set; }
	[Required]
	[DataType(DataType.Password)]
	public string? Password{get;set;}
}