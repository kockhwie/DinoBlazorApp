using System;

namespace DinoAI.Data;

public record EvolutionAge(
    string Key,             // e.g. "stone-age"
    string Name,            // e.g. "Stone Age"
    string Subtitle,        // e.g. "Survival & Instinct"
    string Icon,            // Tabler icon class
    string AccentColor,     // Tailwind/CSS color
    int Order
);

public record KnowledgeCard(
    string Slug,
    string Title,
    string Summary,
    string Body,
    string Category,
    string AgeKey,          // links to EvolutionAge
    string Icon,            // Tabler icon class
    string Tags,            // comma-separated
    int SortOrder
);
