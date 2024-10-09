using System.Net.Http.Json;
using MealOrdering.Shared.DTO;
using MealOrdering.Shared.ResponseModel;
using Microsoft.AspNetCore.Components;

namespace MealOrdering.Client.Pages.Users
{
    public class UserListProcess:ComponentBase
    {

        [Inject]
        public HttpClient client { get; set; }
		protected List<UserDTO> userList = new List<UserDTO>();
		protected override async Task OnInitializedAsync()
		{
			await LoadList();
		}


		protected async Task LoadList()
		{
		var serviceResponse =await client.GetFromJsonAsync<ServiceResponse<List<UserDTO>>>("api/User/Users");

			if (serviceResponse.Success)
			{
			    userList = serviceResponse.Value;
			}
		}

	}
}
