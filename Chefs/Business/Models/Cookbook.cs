using CookbookData = Chefs.Client.Models.CookbookData;
using RecipeData = Chefs.Client.Models.RecipeData;
namespace Chefs.Business.Models;

public partial record Cookbook : IChefEntity
{
	internal Cookbook(CookbookData cookbookData)
	{
		Id = cookbookData.Id ?? Guid.Empty;
		UserId = cookbookData.UserId ?? Guid.Empty;
		Name = cookbookData.Name;
		Recipes = cookbookData.Recipes?
			.Select(c => new Recipe(c))
			.ToImmutableList() ?? ImmutableList<Recipe>.Empty;
		CookbookImages = new CookbookImages(cookbookData.Recipes?.ToImmutableList() ?? ImmutableList<RecipeData>.Empty);
	}

	internal Cookbook() { Recipes = ImmutableList<Recipe>.Empty; }

	public Guid Id { get; init; }
	public Guid UserId { get; init; }
	public string? Name { get; init; }
	public int PinsNumber => Recipes?.Count ?? 0;
	public IImmutableList<Recipe> Recipes { get; init; }
	public CookbookImages? CookbookImages { get; init; }

	internal CookbookData ToData() => new()
	{
		Id = Id,
		UserId = UserId,
		Name = Name,
		Recipes = Recipes?
			.Select(c => c.ToData())
			.ToList()
	};

	internal CookbookData ToData(IImmutableList<Recipe>? recipes) => new()
	{
		Id = Id,
		UserId = UserId,
		Name = Name,
		Recipes = recipes is null
			? Recipes?
				.Select(r => r.ToData())
				.ToList()
			: recipes
				.Select(r => r.ToData())
				.ToList()
	};

	internal static CookbookData CreateData(Guid userId, string name, IImmutableList<Recipe> recipes)
	{
		return new CookbookData
		{
			Id = Guid.NewGuid(),
			Name = name,
			UserId = userId,
			Recipes = recipes?.Select(r => r.ToData()).ToList()
		};
	}

	internal UpdateCookbook UpdateCookbook() => new(this);
}
