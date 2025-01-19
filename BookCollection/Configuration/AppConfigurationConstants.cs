﻿namespace BookCollection.Configuration;

public static class AppConfigurationConstants
{
    public static string BookCollectionFile {  get; set; }
    public static void Initialize(IConfiguration configuration)
    {
        BookCollectionFile = configuration["Files:BookJson"];
    }
}
