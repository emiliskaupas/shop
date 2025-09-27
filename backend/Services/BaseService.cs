using backend.Data;
namespace backend.Services;
public class BaseService
{
    protected readonly AppDbContext shopContext;

    protected BaseService(AppDbContext shopContext)
    {
        this.shopContext = shopContext;
    }
}
