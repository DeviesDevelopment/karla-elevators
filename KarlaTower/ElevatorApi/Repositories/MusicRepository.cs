using System.Collections.Immutable;

namespace KarlaTower.Repositories;

public static class MusicRepository // TODO Build an interface
{
    public static ImmutableDictionary<int, string> ElevatorSongs =>
        ImmutableDictionary.CreateRange(new Dictionary<int, string>
        {
            { 1, "Easy by The Commodores" },
            { 2, "Just the Way You Are by Billy Joel" },
            { 3, "Smooth by Santana ft. Rob Thomas" },
            { 4, "Rock with You by Michael Jackson" },
            { 5, "I Will Always Love You by Whitney Houston" },
            { 6, "The Way You Look Tonight by Frank Sinatra" },
            { 7, "How Deep Is Your Love by Bee Gees" },
            { 8, "I Can't Help Falling in Love by Elvis Presley" },
            { 9, "Can't Get Enough of Your Love, Babe by Barry White" },
            { 10, "Let's Stay Together by Al Green" }
        });
}