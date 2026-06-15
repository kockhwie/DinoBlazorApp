using System;
using System.Collections.Generic;
using System.Linq;
using DinoAI.Data;

namespace DinoAI.Services;

public class KnowledgeService
{
    public IReadOnlyList<EvolutionAge> GetAges()
    {
        return KnowledgeStore.Ages;
    }

    public IReadOnlyList<string> GetCategories()
    {
        return KnowledgeStore.Cards
            .Select(c => c.Category)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c)
            .ToList();
    }

    public IReadOnlyList<KnowledgeCard> GetCards(string? ageKey = null, string? category = null, string? search = null)
    {
        var query = KnowledgeStore.Cards.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(ageKey))
        {
            query = query.Where(c => c.AgeKey.Equals(ageKey, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => 
                c.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Summary.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Body.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Tags.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return query.OrderBy(c => c.SortOrder).ToList();
    }

    public KnowledgeCard? GetCardBySlug(string slug)
    {
        return KnowledgeStore.Cards.FirstOrDefault(c => c.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<KnowledgeCard> GetRelatedCards(string slug, int count = 3)
    {
        var card = GetCardBySlug(slug);
        if (card == null) return Array.Empty<KnowledgeCard>();

        return KnowledgeStore.Cards
            .Where(c => c.Slug != slug && c.Category.Equals(card.Category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(_ => Guid.NewGuid()) // Random selection from same category
            .Take(count)
            .ToList();
    }
}
