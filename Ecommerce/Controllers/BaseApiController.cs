﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // معمول عشان لو فى حاجه شيرد بين كل الكنترولرز
    public class BaseApiController : ControllerBase
    {
    }
}
