interface Edible
{
    int baseNutritionalValue { get; set; }
    bool canBeEaten { get; set; }
    int NutritionalValue();
}
