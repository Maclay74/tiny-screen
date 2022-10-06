using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Common.Framework;
using Common.Interfaces;
using Godot;
using SimpleInjector;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;
using DI = SimpleInjector.Container;

namespace TinyScreen.Framework;

public partial class ContainerNode : Node {
    public SimpleInjector.Container _container;

    public override void _EnterTree() {
        base._EnterTree();
        _container = new SimpleInjector.Container();

        _container.Register<IDatabaseService, EfcDatabaseService>(Lifestyle.Singleton);
        _container.Register<ISettingsService, DatabaseSettingsService>(Lifestyle.Singleton);
        _container.Register<IUpdateService, LocalUpdateService>(Lifestyle.Singleton);
        _container.Register<LibraryService>(Lifestyle.Singleton);
        _container.Register<ImageService>(Lifestyle.Singleton);
        _container.Register<IHardwareService>(HardwareFactory.GetHardwareService, Lifestyle.Singleton);
        LoadPlugins();
        LoadNodes();
    }
    
    [Injector]
    private DI ResolveDependencies() {
        // Get container for DI
        var containerNodePath = "/root/AutoloadScene";
        return GetNode<ContainerNode>(containerNodePath)._container;
    }

    private void LoadPlugins() {
        List<Assembly> sources = new List<Assembly>();
        
        var currentContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
        if (currentContext == null) return;

        if (Directory.Exists(ProjectSettings.GlobalizePath("Plugins"))) {
            foreach (var dll in Directory.GetFiles(ProjectSettings.GlobalizePath("Plugins"), "*.dll")) {
                Assembly plugin = currentContext.LoadFromAssemblyPath(Path.GetFullPath(dll));
                sources.Add(plugin);
            }
        }

        _container.Collection.Register<ILibrarySource>(sources);
        _container.Collection.Register<IGameDataProvider>(sources);
    }

    private void LoadNodes() {
        // Modal
        var modalService = GetNode<ModalService>("Modal");
        _container.RegisterInstance<ModalService>(modalService);
    }
}