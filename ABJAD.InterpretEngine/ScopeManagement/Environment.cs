﻿using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class Environment : ScopeFacade
{
    private readonly List<Scope> scopes;

    public Environment(List<Scope> scopes)
    {
        this.scopes = scopes;
    }

    public bool ReferenceExists(string name)
    {
        return scopes.Any(s => s.ReferenceScope.ReferenceExists(name));
    }

    public bool ReferenceExistsInCurrentScope(string name)
    {
        return scopes.Last().ReferenceScope.ReferenceExists(name);
    }

    public DataType GetReferenceType(string name)
    {
        return scopes.FindLast(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.GetType(name);
    }

    public object GetReference(string name)
    {
        return scopes.FindLast(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.Get(name);
    }

    public void SetReference(string name, object value)
    {
        if (scopes.Last().ReferenceScope.ReferenceExists(name))
        {
            scopes.Last().ReferenceScope.Update(name, value);
        }
        else
        {
            var referenceType = scopes.FindLast(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.GetType(name);
            scopes.Last().ReferenceScope.DefineVariable(name, referenceType, value);
        }
    }

    public void DefineVariable(string name, DataType type, object value)
    {
        scopes.Last().ReferenceScope.DefineVariable(name, type, value);
    }

    public void DefineConstant(string name, DataType type, object value)
    {
        scopes.Last().ReferenceScope.DefineConstant(name, type, value);
    }

    public bool IsReferenceConstant(string name)
    {
        return scopes.FindLast(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.IsConstant(name);
    }

    public bool FunctionExists(string name, int numberOfParameters)
    {
        return scopes.Any(s => s.FunctionScope.FunctionExists(name, numberOfParameters));
    }

    public bool FunctionExistsInCurrentScope(string name, int numberOfParameters)
    {
        return scopes.Last().FunctionScope.FunctionExists(name, numberOfParameters);
    }

    public DataType? GetFunctionReturnType(string name, int numberOfParameters)
    {
        return scopes
            .Last(s => s.FunctionScope.FunctionExists(name, numberOfParameters)).FunctionScope
            .GetFunctionReturnType(name, numberOfParameters);
    }

    public FunctionElement GetFunction(string name, int numberOfParameters)
    {
        return scopes
            .Last(s => s.FunctionScope.FunctionExists(name, numberOfParameters)).FunctionScope
            .GetFunction(name, numberOfParameters);
    }

    public void DefineFunction(string name, FunctionElement function)
    {
        scopes.Last().FunctionScope.DefineFunction(name, function);
    }

    public ScopeFacade CloneScope()
    {
        return new Environment(scopes.Select(s => new Scope(s.ReferenceScope.Clone(), s.FunctionScope.Clone())).ToList());
    }

    public void AddNewScope()
    {
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var functionScope = new FunctionScope(new Dictionary<(string, int), FunctionElement>());
        scopes.Add(new Scope(referenceScope, functionScope));
    }
}