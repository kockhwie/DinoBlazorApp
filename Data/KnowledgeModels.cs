namespace DinoAI.Data;

public record EvolutionAge(
    string Key,
    string Name,
    string Subtitle,
    string Icon,
    string AccentColor,
    int Order
);

public record KnowledgeCard(
    string Slug,
    string Title,
    string Summary,
    string WhyCare,
    string Example,
    string WithoutIt,
    string Body,
    string Category,
    string AgeKey,
    string Icon,
    string Tags,
    int SortOrder,
    string? ShortTitle = null
);