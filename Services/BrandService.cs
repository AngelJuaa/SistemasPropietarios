using Backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services;

public class BrandService : ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>
{
    private readonly IRepository<Brand> _brandRepository;
    private readonly IRepository<Beer> _beerRepository;

    public BrandService(IRepository<Brand> brandRepository, IRepository<Beer> beerRepository)
    {
        _brandRepository = brandRepository;
        _beerRepository = beerRepository;
    }

    public async Task<IEnumerable<BrandDto>> Get()
    {
        var brands = await _brandRepository.Get();
        return brands.Select(b => new BrandDto
        {
            Id = b.BrandId,
            Name = b.Name
        });
    }

    public async Task<BrandDto> GetById(int id)
    {
        var brand = await _brandRepository.GetById(id);
        if (brand == null)
        {
            return null;
        }

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name
        };
    }

    public async Task<BrandDto> Add(BrandInsertDto brandInsertDto)
    {
        var normalizedName = brandInsertDto.Name.Trim();
        var exists = (await _brandRepository.Get())
            .Any(b => b.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException("Ya existe una marca con ese nombre");
        }

        var brand = new Brand
        {
            Name = normalizedName
        };

        await _brandRepository.Add(brand);
        await _brandRepository.Save();

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name
        };
    }

    public async Task<BrandDto> Update(int id, BrandUpdateDto brandUpdateDto)
    {
        var brand = await _brandRepository.GetById(id);
        if (brand == null)
        {
            return null;
        }

        var normalizedName = brandUpdateDto.Name.Trim();
        var exists = (await _brandRepository.Get())
            .Any(b => b.BrandId != id && b.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException("Ya existe una marca con ese nombre");
        }

        brand.Name = normalizedName;
        _brandRepository.Update(brand);
        await _brandRepository.Save();

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name
        };
    }

    public async Task<BrandDto> Delete(int id)
    {
        var brand = await _brandRepository.GetById(id);
        if (brand == null)
        {
            return null;
        }

        var hasBeers = (await _beerRepository.Get()).Any(b => b.BrandId == id);
        if (hasBeers)
        {
            throw new InvalidOperationException("No se puede eliminar una marca con cervezas asociadas");
        }

        _brandRepository.Delete(brand);
        await _brandRepository.Save();

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name
        };
    }

    public async Task<IEnumerable<BeerDto>> GetBrandBeers(int brandId)
    {
        var beers = await _beerRepository.Get();
        return beers
            .Where(b => b.BrandId == brandId)
            .Select(b => new BeerDto
            {
                Id = b.BeerId,
                Name = b.Name,
                Alcohol = b.Alcohol,
                BrandId = b.BrandId
            });
    }
}
