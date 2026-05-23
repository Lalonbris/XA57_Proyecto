using Microsoft.AspNetCore.Identity;
using Moq;
using XA57_Proyecto.Application.Services;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Tests.Services;

public class UsuarioAdminServiceTests
{
    private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IRoleStore<IdentityRole>> _roleStoreMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly UsuarioAdminService _service;

    public UsuarioAdminServiceTests()
    {
        _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            _userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            _roleStoreMock.Object, null!, null!, null!, null!);

        _service = new UsuarioAdminService(_userManagerMock.Object, _roleManagerMock.Object);
    }

    // ============ CrearUsuarioAsync ============

    [Fact]
    public async Task CrearUsuarioAsync_EmailVacio_RetornaError()
    {
        var user = new ApplicationUser { Email = "" };

        var result = await _service.CrearUsuarioAsync(user, "Pass1!");

        Assert.False(result.Succeeded);
        Assert.Contains("correo", result.Errors.First().Description.ToLower());
    }

    [Fact]
    public async Task CrearUsuarioAsync_EmailInvalido_RetornaError()
    {
        var user = new ApplicationUser { Email = "notanemail" };

        var result = await _service.CrearUsuarioAsync(user, "Pass1!");

        Assert.False(result.Succeeded);
        Assert.Contains("formato", result.Errors.First().Description.ToLower());
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name+tag@domain.co")]
    [InlineData("a@b.cd")]
    public async Task CrearUsuarioAsync_EmailValido_LlamaUserManager(string email)
    {
        var user = new ApplicationUser { Email = email };
        _userManagerMock.Setup(m => m.CreateAsync(user, "Pass1!"))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _service.CrearUsuarioAsync(user, "Pass1!");

        Assert.True(result.Succeeded);
        _userManagerMock.Verify(m => m.CreateAsync(user, "Pass1!"), Times.Once);
    }

    // ============ ActualizarUsuarioAsync ============

    [Fact]
    public async Task ActualizarUsuarioAsync_EmailVacio_RetornaError()
    {
        var user = new ApplicationUser { Email = "" };

        var result = await _service.ActualizarUsuarioAsync(user);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task ActualizarUsuarioAsync_EmailValido_LlamaUserManager()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        _userManagerMock.Setup(m => m.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _service.ActualizarUsuarioAsync(user);

        Assert.True(result.Succeeded);
        _userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
    }

    // ============ EliminarUsuarioAsync ============

    [Fact]
    public async Task EliminarUsuarioAsync_NoExiste_RetornaError()
    {
        _userManagerMock.Setup(m => m.FindByIdAsync("nonexistent"))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _service.EliminarUsuarioAsync("nonexistent");

        Assert.False(result.Succeeded);
        Assert.Contains("no encontrado", result.Errors.First().Description.ToLower());
    }

    [Fact]
    public async Task EliminarUsuarioAsync_Existe_LlamaDelete()
    {
        var user = new ApplicationUser { Id = "user1", Email = "test@test.com" };
        _userManagerMock.Setup(m => m.FindByIdAsync("user1")).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _service.EliminarUsuarioAsync("user1");

        Assert.True(result.Succeeded);
        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
    }

    // ============ AgregarRolAsync ============

    [Fact]
    public async Task AgregarRolAsync_UsuarioNoExiste_RetornaError()
    {
        _userManagerMock.Setup(m => m.FindByIdAsync("nonexistent"))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _service.AgregarRolAsync("nonexistent", "Admin");

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task AgregarRolAsync_Existe_LlamaAddToRole()
    {
        var user = new ApplicationUser { Id = "user1" };
        _userManagerMock.Setup(m => m.FindByIdAsync("user1")).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.AddToRoleAsync(user, "Admin"))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _service.AgregarRolAsync("user1", "Admin");

        Assert.True(result.Succeeded);
    }

    // ============ QuitarRolAsync ============

    [Fact]
    public async Task QuitarRolAsync_UsuarioNoExiste_RetornaError()
    {
        _userManagerMock.Setup(m => m.FindByIdAsync("nonexistent"))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _service.QuitarRolAsync("nonexistent", "Admin");

        Assert.False(result.Succeeded);
    }

    // ============ ObtenerPorEmailAsync ============

    [Fact]
    public async Task ObtenerPorEmailAsync_LlamaFindByEmail()
    {
        var user = new ApplicationUser { Id = "1", Email = "test@test.com" };
        _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        var result = await _service.ObtenerPorEmailAsync("test@test.com");

        Assert.Same(user, result);
    }
}
