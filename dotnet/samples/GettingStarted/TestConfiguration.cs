﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using RepoUtils;

public sealed class TestConfiguration
{
    private readonly IConfigurationRoot _configRoot;
    private static TestConfiguration? s_instance;

    private TestConfiguration(IConfigurationRoot configRoot)
    {
        this._configRoot = configRoot;
    }

    public static void Initialize(IConfigurationRoot configRoot)
    {
        s_instance = new TestConfiguration(configRoot);
    }

    public static OpenAIConfig OpenAI => LoadSection<OpenAIConfig>();

    private static T LoadSection<T>([CallerMemberName] string? caller = null)
    {
        if (s_instance == null)
        {
            throw new InvalidOperationException(
                "TestConfiguration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
        }

        if (string.IsNullOrEmpty(caller))
        {
            throw new ArgumentNullException(nameof(caller));
        }

        return s_instance._configRoot.GetSection(caller).Get<T>() ??
               throw new ConfigurationNotFoundException(section: caller);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    public class OpenAIConfig
    {
        public string ModelId { get; set; }
        public string ChatModelId { get; set; }
        public string EmbeddingModelId { get; set; }
        public string ApiKey { get; set; }
    }
}
