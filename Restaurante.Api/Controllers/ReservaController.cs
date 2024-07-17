using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Api.Context;
using Restaurante.Api.Functions;
using Restaurante.Api.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Restaurante.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("criar-reserva")]
        public IActionResult Create([FromBody] ReservaBody reservaBase)
        {
            if (reservaBase.QuantidadeDePessoas < 1)
                return BadRequest("Número de pessoas inválido.");

            if (!Email.VerifyMail(reservaBase.EmailCliente))
                return BadRequest($"O email \"{reservaBase.EmailCliente}\" é inválido");
            
            if (!Phone.VerifyPhone(reservaBase.TelefoneCliente, out string telefoneFormatado))
                return BadRequest($"O telefone \"{reservaBase.TelefoneCliente}\" é inválido.");

            reservaBase.TelefoneCliente = telefoneFormatado;

            // funcao de dois parametros para arrendondar o tempo para o mais proximo.
            int hora = RoundTime(reservaBase.DataHoraReserva.Hour, reservaBase.DataHoraReserva.Minute, out int minuto);

            var newDate = new DateTime(reservaBase.DataHoraReserva.Year, reservaBase.DataHoraReserva.Month, reservaBase.DataHoraReserva.Day, hora, minuto, 0);

            reservaBase.DataHoraReserva = newDate;

            int idMesaDisponivel = FindMesa(reservaBase);

            if (idMesaDisponivel == -1)
                return BadRequest("Não temos mesas disponíveis.");

            Reserva reserva = new() {
                NomeCliente = reservaBase.NomeCliente,
                EmailCliente = reservaBase.EmailCliente,
                TelefoneCliente = reservaBase.TelefoneCliente,
                IdMesa = idMesaDisponivel,
                DataHoraReserva = reservaBase.DataHoraReserva,
                QuantidadeDePessoas = reservaBase.QuantidadeDePessoas
            };

            // TO-DO: colocar script para enviar mensagem pro usuario
            string msg = $"👋 Olá {reserva.NomeCliente}, sua reserva para o dia {reserva.DataHoraReserva:dd/MM/yyyy} " + 
            "Foi realizada com sucesso! Atente-se aos detalhes abaixo:\n\n" +
            $"🕛 *Data e Hora:* {reserva.DataHoraReserva}\n" + 
            $"🔢 *Número da mesa:* {reserva.IdMesa}\n" +
            $"👥 *Quantidade de pessoas*: {reserva.QuantidadeDePessoas}\n\n" + 
            "_Chegue com antecedência e evite transtornos!_";

            try
            {
                EnviarNotificacaoSMS(reserva.TelefoneCliente, msg);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro inesperado. Sua reserva não poderá ser realizada.\n Detalhes do erro: " + ex);
            }

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return Ok("Sua reserva foi realizada com sucesso. Número da sua mesa: " + reserva.IdMesa);
        }

        private int FindMesa(ReservaBody reserva)
        {
            // verifica se tem mesas disponiveis de forma automatica, passando o numero (id) da mesa pro usuario
            List<Mesa> mesas = _context.Mesas.Where(c => c.Capacidade == reserva.QuantidadeDePessoas).ToList();
            
            int idMesaDisponivel = -1;
            if (mesas == null)
                return -1;

            foreach (Mesa mesa in mesas)
            {
                /* TO-DO: fazer a verificação de duas horas de diferença para permitir a 
                 * criação de uma nova reserva na mesma mesa em horários diferentes */

                if (!_context.Reservas.Any(r => r.IdMesa == mesa.Id && r.DataHoraReserva == reserva.DataHoraReserva))
                {
                    idMesaDisponivel = mesa.Id;
                    return idMesaDisponivel;
                }
            }
            return -1;
        }
        private static void EnviarNotificacaoSMS(string telefone, string mensagem)
        {
            DotEnv.Load();

            var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            var phoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{telefone}")) {
                From = new PhoneNumber(phoneNumber),
                Body = mensagem
            };

            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
        }
    
        private static int RoundTime(int hora, int minuto, out int _minuto)
        {
            if (minuto < 15)
            {
                minuto = 0;
            }
            else if (minuto > 39)
            {
                hora += 1;
                minuto = 0;
            }
            else
            {
                minuto = 30;
            }

            _minuto = minuto;
            return hora;
        }
    }
}