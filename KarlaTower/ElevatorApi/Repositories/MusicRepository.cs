using System.Collections.Immutable;

namespace KarlaTower.Repositories;

public static class MusicRepository
{
    public static ImmutableArray<string> ElevatorSongs => new()
    {
        "Easy by The Commodores",
        "Just the Way You Are by Billy Joel",
        "Smooth by Santana ft. Rob Thomas",
        "Rock with You by Michael Jackson",
        "I Will Always Love You by Whitney Houston",
        "The Way You Look Tonight by Frank Sinatra",
        "How Deep Is Your Love by Bee Gees",
        "I Can't Help Falling in Love by Elvis Presley",
        "Can't Get Enough of Your Love, Babe by Barry White",
        "Let's Stay Together by Al Green",
        "Isn't She Lovely by Stevie Wonder",
        "I'm Yours by Jason Mraz",
        "Crazy by Gnarls Barkley",
        "Kiss by Prince",
        "Smooth Operator by Sade",
        "My Girl by The Temptations",
        "Under Pressure by Queen and David Bowie",
        "I Want to Hold Your Hand by The Beatles",
        "All of Me by John Legend",
        "Stayin' Alive by Bee Gees"
    };
}