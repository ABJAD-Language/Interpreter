﻿using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class Scope : IScope
{
    private readonly IDictionary<string, StateElement> state;

    public Scope(IDictionary<string, StateElement> state)
    {
        this.state = state;
    }

    public bool ReferenceExists(string name)
    {
        return state.ContainsKey(name);
    }

    public DataType GetType(string name)
    {
        return state[name].Type;
    }

    public object Get(string name)
    {
        return state[name].Value;
    }

    public void Set(string name, object value)
    {
        var oldElement = state[name];
        state.Remove(name);
        state.Add(name, oldElement with { Value = value });
    }

    public void Define(string name, DataType type, object value)
    {
        state.Add(name, new StateElement { Type = type, Value = value });
    }

    public IScope Clone()
    {
        return new Scope(state.ToDictionary(entry => entry.Key, entry => entry.Value));
    }
}