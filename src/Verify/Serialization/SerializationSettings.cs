﻿// ReSharper disable UseObjectOrCollectionInitializer

using Formatting = Argon.Formatting;

partial class SerializationSettings
{
    static ArgonJArrayConverter argonJArrayConverter = new();
    static ArgonJObjectConverter argonJObjectConverter = new();
    static NewtonsoftJArrayConverter newtonsoftJArrayConverter = new();
    static NewtonsoftJObjectConverter newtonsoftJObjectConverter = new();
    static FileInfoConverter fileInfoConverter = new();
    static KeyValuePairConverter keyValuePairConverter = new();
#if NET6_0_OR_GREATER
    static TimeConverter timeConverter = new();
    static DateConverter dateConverter = new();
#endif
    static DirectoryInfoConverter directoryInfoConverter = new();
    static StringEnumConverter stringEnumConverter = new();
    static DelegateConverter delegateConverter = new();
    static TargetInvocationExceptionConverter targetInvocationExceptionConverter = new();
    static ExpressionConverter expressionConverter = new();
    static TypeJsonConverter typeJsonConverter = new();
    static MethodInfoConverter methodInfoConverter = new();
    static FieldInfoConverter fieldInfoConverter = new();
    static ConstructorInfoConverter constructorInfoConverter = new();
    static ParameterInfoConverter parameterInfoConverter = new();
    static VersionConverter versionConverter = new();
    static EncodingConverter encodingConverter = new();
    static PropertyInfoConverter propertyInfoConverter = new();
    static ClaimConverter claimConverter = new();
    static AggregateExceptionConverter aggregateExceptionConverter = new();
    static ClaimsPrincipalConverter claimsPrincipalConverter = new();
    static ClaimsIdentityConverter claimsIdentityConverter = new();
    static NameValueCollectionConverter nameValueCollectionConverter = new();
    static StringBuilderConverter stringBuilderConverter = new();
    static TaskConverter taskConverter = new();
    static ValueTaskConverter valueTaskConverter = new();
    static StringWriterConverter stringWriterConverter = new();
    //static DictionaryConverter dictionaryConverter = new();

    JsonSerializerSettings jsonSettings;

    public SerializationSettings()
    {
        IgnoreMembersThatThrow<NotImplementedException>();
        IgnoreMembersThatThrow<NotSupportedException>();
        IgnoreMembers<Exception>("Source", "HResult");
        IgnoreMembersWithType<Stream>();

        jsonSettings = BuildSettings();
    }

    public SerializationSettings(SerializationSettings settings)
    {
        ignoredMembers = settings.ignoredMembers.ToDictionary(
            _ => _.Key,
            _ => _.Value.Clone());
        ignoredByNameMembers = settings.ignoredByNameMembers.Clone();
        ignoreEmptyCollections = settings.ignoreEmptyCollections;
        extraSettings = settings.extraSettings.Clone();
        ignoreMembersThatThrow = settings.ignoreMembersThatThrow.Clone();
        ignoredTypes = settings.ignoredTypes.Clone();
        ignoredInstances = settings.ignoredInstances
            .ToDictionary(
                _ => _.Key,
                _ => _.Value.Clone());
        scrubDateTimes = settings.scrubDateTimes;
        scrubGuids = settings.scrubGuids;
        includeObsoletes = settings.includeObsoletes;

        jsonSettings = BuildSettings();
    }

    bool scrubGuids = true;

    public void DontScrubGuids() =>
        scrubGuids = false;

    bool scrubDateTimes = true;

    public void DontScrubDateTimes() =>
        scrubDateTimes = false;

    JsonSerializerSettings BuildSettings()
    {
        #region defaultSerialization

        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        #endregion

        settings.SerializationBinder = ShortNameBinder.Instance;

        settings.ContractResolver = new CustomContractResolver(this);
        var converters = settings.Converters;
        converters.Add(aggregateExceptionConverter);
        converters.Add(stringBuilderConverter);
        converters.Add(stringWriterConverter);
#if NET6_0_OR_GREATER
        converters.Add(dateConverter);
        converters.Add(timeConverter);
#endif
        converters.Add(fileInfoConverter);
        converters.Add(directoryInfoConverter);
        converters.Add(stringEnumConverter);
        converters.Add(expressionConverter);
        converters.Add(delegateConverter);
        converters.Add(targetInvocationExceptionConverter);
        converters.Add(versionConverter);
        converters.Add(encodingConverter);
        converters.Add(typeJsonConverter);
        converters.Add(methodInfoConverter);
        converters.Add(fieldInfoConverter);
        converters.Add(constructorInfoConverter);
        converters.Add(propertyInfoConverter);
        converters.Add(parameterInfoConverter);
        converters.Add(claimConverter);
        converters.Add(claimsIdentityConverter);
        converters.Add(taskConverter);
        converters.Add(valueTaskConverter);
        converters.Add(claimsPrincipalConverter);
        //converters.Add(dictionaryConverter);
        converters.Add(argonJArrayConverter);
        converters.Add(argonJObjectConverter);
        converters.Add(newtonsoftJArrayConverter);
        converters.Add(newtonsoftJObjectConverter);
        converters.Add(nameValueCollectionConverter);
        converters.Add(keyValuePairConverter);
        foreach (var extraSetting in extraSettings)
        {
            extraSetting(settings);
        }

        return settings;
    }

    public void AddExtraSettings(Action<JsonSerializerSettings> action)
    {
        extraSettings.Add(action);
        action(jsonSettings);
        serializer = null;
    }

    List<Action<JsonSerializerSettings>> extraSettings = new();
    JsonSerializer? serializer;

    internal JsonSerializer Serializer
    {
        get
        {
            var jsonSerializer = serializer;
            if (jsonSerializer == null)
            {
                return serializer = JsonSerializer.Create(jsonSettings);
            }

            return jsonSerializer;
        }
    }

    internal bool SortDictionaries = true;

    public void DontSortDictionaries() =>
        SortDictionaries = false;
}