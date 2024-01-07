using AutoMapper;
using PspPos.Models;

namespace PspPos.Helpers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Sample, SampleViewModel>();
        CreateMap<SamplePostModel, Sample>();

        CreateMap<Company, CompanyViewModel>();
        CreateMap<CompanyPostModel, Company>();

        CreateMap<OrderPostModel, Order>();

        CreateMap<UserPostModel, User>();

        CreateMap<ItemPostModel, Item>();
        CreateMap<Item, ItemViewModel>();

        CreateMap<PaymentPostModel, Payment>();
        
        CreateMap<StoreCreate, Store>();
        CreateMap<Store, StoreCreate>();
        CreateMap<StoreUpdate, Store>();
 
        CreateMap<ItemOptionPostModel, ItemOption>();
        CreateMap<ItemOption, ItemOptionViewModel>();

        CreateMap<InventoryPostModel, Inventory>();
        CreateMap<Inventory, InventoryViewModel>();

        CreateMap<OrderItemPostModel, OrderItem>();
        CreateMap<OrderItem, OrderItemViewModel>();
        CreateMap<OrderItem, OrderItemPostModel> ();

    }
}

