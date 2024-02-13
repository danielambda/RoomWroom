using Domain.RecipeAggregate.ValueObjects;

namespace Domain.RecipeAggregate;

public class Recipe : AggregateRoot<RecipeId>
{
    public string Title { get; }
    public string Description { get; }
    public int Servings { get; }
    public int ReadyInMinutes { get; }
    
    public IReadOnlyList<MeasuredIngredient> Ingredients => _ingredients.AsReadOnly();
    
    private readonly List<MeasuredIngredient> _ingredients;

    private Recipe(RecipeId id, string title, string description, List<MeasuredIngredient> ingredients,
        int servings, int readyInMinutes)
    {
        Title = title;
        Description = description;
        _ingredients = ingredients;
        Servings = servings;
        ReadyInMinutes = readyInMinutes;
    }

    public static Recipe Create(RecipeId id, string title, string description, List<MeasuredIngredient> ingredients,
        int servings, int readyInMinutes)
        => new(id, title, description, ingredients, servings, readyInMinutes);

    public static Recipe CreateNew(string title, string description, List<MeasuredIngredient> ingredients,
        int servings, int readyInMinutes)
        => new(RecipeId.CreateUnique(), title, description, ingredients, servings, readyInMinutes);

}