using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.DTOs;
using XA57_Proyecto.Application.Interfaces;
using XA57_Proyecto.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace XA57_Proyecto.Controllers
{
    [Route("Carrito")]
    public class CarritoController : Controller
    {
        private readonly IPedidoService _pedidoService;

        public CarritoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return View(new CarritoViewModel { Items = new List<Domain.Entities.Pedido>() });
            }
            
            var items = await _pedidoService.ObtenerCarritoAsync(userId);
            var viewModel = new CarritoViewModel { Items = items };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost("Agregar")]
        public async Task<IActionResult> Agregar([FromBody] CarritoItemDto item)
        {
            if (item == null || item.ProductoId == 0)
                return BadRequest(new { mensaje = "Datos inválidos." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var resultado = await _pedidoService.AgregarAsync(item, userId);
            return Ok(new { mensaje = resultado.Mensaje, pedidoId = resultado.PedidoId });
        }

        [HttpPost("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _pedidoService.EliminarItemAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Cantidad")]
        public async Task<IActionResult> Cantidad()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(0);
            }
            
            var count = await _pedidoService.ContarItemsAsync(userId);
            return Ok(count);
        }

        [Authorize]
        [HttpGet("DescargarHojaAyuda")]
        public async Task<IActionResult> DescargarHojaAyuda()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var items = await _pedidoService.ObtenerCarritoAsync(userId);

            if (!items.Any()) return BadRequest("El carrito está vacío");

            var total = items.Sum(i => (i.Producto?.Precio ?? 0) * i.Cantidad);
            var numeroCuenta = "1234567890"; // Número random de prueba

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("XA57 Autobuses").FontSize(24).SemiBold().FontColor(Colors.Grey.Darken4);
                            column.Item().Text("Hoja de Ayuda para Pago").FontSize(14).FontColor(Colors.Grey.Medium);
                            column.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10).FontColor(Colors.Grey.Medium);
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                    {
                        column.Spacing(20);

                        column.Item().Text("Instrucciones de Pago").FontSize(16).SemiBold();
                        
                        column.Item().Background(Colors.Grey.Lighten4).Padding(15).Column(inst => 
                        {
                            inst.Spacing(5);
                            inst.Item().Text("Por favor, realice una transferencia interbancaria con los siguientes datos:");
                            inst.Item().Text($"Banco: Banco de Prueba S.A.").SemiBold();
                            inst.Item().Text($"Beneficiario: XA57 Proyecto").SemiBold();
                            inst.Item().Text($"CLABE: {numeroCuenta}").SemiBold().FontColor(Colors.Blue.Darken2);
                            inst.Item().Text($"Monto a pagar: ${total:N2} MXN").SemiBold().FontSize(14);
                            inst.Item().Text("Concepto: Pago Pedido XA57");
                        });

                        column.Item().Text("Resumen del Pedido").FontSize(14).SemiBold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(1).PaddingBottom(5).Text("Producto");
                                header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Precio Unit.");
                                header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Cant.");
                                header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Subtotal");
                            });

                            foreach (var item in items)
                            {
                                var precio = item.Producto?.Precio ?? 0;
                                table.Cell().PaddingVertical(5).Text(item.Producto?.Nombre ?? "N/A");
                                table.Cell().PaddingVertical(5).AlignRight().Text($"${precio:N2}");
                                table.Cell().PaddingVertical(5).AlignRight().Text($"{item.Cantidad}");
                                table.Cell().PaddingVertical(5).AlignRight().Text($"${(precio * item.Cantidad):N2}");
                            }
                        });
                        
                        column.Item().AlignRight().Text($"Total a pagar: ${total:N2}").FontSize(16).SemiBold();
                    });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Hoja_Ayuda_Pago_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [Authorize]
        [HttpPost("ConfirmarPago")]
        public async Task<IActionResult> ConfirmarPago()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var items = await _pedidoService.ObtenerCarritoAsync(userId);

            foreach (var item in items)
            {
                await _pedidoService.ActualizarEstadoAsync(item.Id, "Pagado");
            }

            return Ok(new { mensaje = "Pago registrado correctamente" });
        }
    }
}

