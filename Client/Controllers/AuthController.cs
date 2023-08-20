using Client.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using SoapWebServiceReference;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {

            return View("Register");
        }

        public async Task<IActionResult> Register()
        {
            return View("Register");
        }

        public async Task<IActionResult> Login()
        {
            return View("Login");
        }
        public async Task<IActionResult> RegisterUser(RegisterDTOS registerDTOS)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage).FirstOrDefault();
            }
            else
            {
                if(!registerDTOS.Password.Equals(registerDTOS.ConfirmPassword))
                {
                    ViewBag.ErrorMessage = "Passwords entered must be same.";
                }
                else
                {
                    IService iAuthService = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService_soap);
                    var result = await iAuthService.RegisterUserAsync(new RegisterUserRequest()
                    {
                        Body = new RegisterUserRequestBody()
                        {
                            email = registerDTOS.Email,
                            password = registerDTOS.Password,
                            phone = "",
                            username = registerDTOS.Username
                        }
                    });
                    bool isSuccess = false;
                    if (result != null && result.Body != null && result.Body.RegisterUserResult != null)
                    {
                        if (result.Body.RegisterUserResult.StatusCode == 200)
                        {
                            isSuccess = true;
                        }
                        ViewBag.ErrorMessage = result.Body.RegisterUserResult.StatusMessage;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Unable to process your request.";
                    }
                   
                    if (isSuccess)
                    {
                        return RedirectToAction("Register");
                    }
                }
                
            }    
            
            return View("Register", new RegisterDTOS());
        }
        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage).FirstOrDefault();
            }
            else
            {
                IService iAuthService = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService_soap);
                var result = await iAuthService.LoginAsync(new LoginRequest()
                {
                     Body=new LoginRequestBody()
                     {
                          password=loginDTO.Password,
                           username= loginDTO.Email
                     }
                }
                );
                bool isSuccess = false;
                if (result != null && result.Body != null && result.Body.LoginResult != null)
                {
                    if (result.Body.LoginResult.StatusCode == 200)
                    {
                        isSuccess = true;
                    }
                    ViewBag.ErrorMessage = result.Body.LoginResult.StatusMessage;
                }
                else
                {
                    ViewBag.ErrorMessage = "Unable to process your request.";
                }
                if (isSuccess)
                {
                    if (loginDTO.Email != "admin@gmail.com")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("GetUsers");
                    }
                }

            }
            return View("Login");
        }

        public async Task<IActionResult> GetUsers()
        {
            IService iAuthService = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService_soap);
            var result= await iAuthService.GetUsersAsync();
            List<UsersDTO> usersData = new List<UsersDTO>();
            foreach (var item in result)
            {
                usersData.Add(new UsersDTO() { UserName = item.Username, CreatedDate = DateTime.UtcNow, Email = item.Email, UserId = item.Id.ToString(), Password = "" });
            }
            return View("Getusers", usersData);
        }

    }
}
