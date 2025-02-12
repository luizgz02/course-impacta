﻿using TesteJWT.Dto;
using TesteJWT.Models;
using TesteJWT.Services;
using TesteJWT.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TesteJWTDBContext _context;

        public LoginController(TesteJWTDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Login) || string.IsNullOrWhiteSpace(dto.Login)
                    || string.IsNullOrEmpty(dto.Senha) || string.IsNullOrWhiteSpace(dto.Senha))
                {
                    return BadRequest("Parâmetros de entrada inválidos");
                }

                var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Login.Trim().ToLower()
                        || u.Senha == PasswordUtil.GeneratePassword(dto.Senha));

                if (usuario == null)
                {
                    return BadRequest("Parâmetros de entrada inválidos");
                }

                var token = TokenService.GenerateToken(usuario);
                return Ok(new
                {
                    Email = usuario.Email,
                    Token = token
                });
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível autenticar, tente novamente");
            }
        }
    }
}
