using System.Reflection;
using Patterns.Core.Algorithms;

namespace Patterns.Core;

public class AlgorithmFactory
{
    public IAlgorithm Create(AlgorithmType type, IDictionary<string, string> args)
    {
        return type switch
        {
            AlgorithmType.Life => this.CreateWithArguments<Life>(args),
            AlgorithmType.Flame => this.CreateWithArguments<Flame>(args),
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };
    }

    private IAlgorithm CreateWithArguments<TAlgorithm>(IDictionary<string, string> args)
        where TAlgorithm : IAlgorithm
    {
        ConstructorInfo constructorInfo = typeof(TAlgorithm).GetConstructors().Single();
        List<object?> arguments = new();
        foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
        {
            if (parameterInfo.Name == null)
            {
                throw new Exception($"Constructor of type {typeof(TAlgorithm)} contains parameter without name.");
            }

            arguments.Add(
                args.ContainsKey(parameterInfo.Name) ?
                    Convert.ChangeType(args[parameterInfo.Name], parameterInfo.ParameterType) :
                    parameterInfo.DefaultValue);
        }

        return (IAlgorithm)constructorInfo.Invoke(arguments.ToArray());
    }
}
