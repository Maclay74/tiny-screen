using System;
using System.Linq;
using System.Reflection;
using Godot;
using SimpleInjector;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;
using Container = SimpleInjector.Container;
using HarmonyLib;
using TinyScreen.Framework.Attributes;

public class ContainerNode : Node {
    
    public Container _container;

    public override void _EnterTree() {
        base._EnterTree();
        
        _container = new Container();
        _container.Register<IDatabaseService, WatsonDatabaseService>(Lifestyle.Singleton);
        _container.Register<ISettingsInterface, DatabaseSettingsService>(Lifestyle.Singleton);
        _container.Register<IUpdateInterface, LocalUpdateService>(Lifestyle.Singleton);
        _container.Register<IHardwareService>(HardwareFactory.GetHardwareService, Lifestyle.Singleton);
        InjectDI();

    }

    /**
     * This function patches original Node so when user calls _Ready method from the base class
     * it injects all the dependencies to it
     */
    private void InjectDI() {
        var harmony = new Harmony("com.tinyscreen.di");

        var mOriginal = AccessTools.Method(typeof(Node), nameof(_Ready)); // if possible use nameof() here
        var mPostfix = SymbolExtensions.GetMethodInfo(() => ResolveDependencies(this));
        
        harmony.Patch(mOriginal, postfix: new HarmonyMethod(mPostfix));
    }

    private static void ResolveDependencies(Node __instance) {
        
        // Get container for DI
        var containerNodePath = "/root/ContainerNode";
        var containerNode = __instance.GetNode<ContainerNode>(containerNodePath);
        
        // Cache type of attribute
        var at = typeof(InjectAttribute);

        // Find injectable fields
        var fields = __instance.GetType()
            .GetRuntimeFields()
            .Where(f => f.GetCustomAttributes(at, true).Any());

        // Recursively inject
        foreach (var field in fields) {
            var obj = containerNode._container.GetInstance(field.FieldType);

            try {
                field.SetValue(__instance, obj);
            }
            catch (InvalidCastException) {
                GD.PrintErr($"Error converting value {obj} ({obj.GetType()}) to {field.FieldType}");
            }
        }
        
    }
}
