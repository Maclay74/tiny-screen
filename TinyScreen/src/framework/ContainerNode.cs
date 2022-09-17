using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using HarmonyLib;
using SimpleInjector;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;
using Container = SimpleInjector.Container;

namespace TinyScreen.Framework {
    public class ContainerNode : Node {
        
        private Container _container;

        public override void _EnterTree() {
            base._EnterTree();

            _container = new Container();
            _container.Register<IDatabaseService, WatsonDatabaseService>(Lifestyle.Singleton);
            _container.Register<ISettingsService, DatabaseSettingsService>(Lifestyle.Singleton);
            _container.Register<IUpdateService, LocalUpdateService>(Lifestyle.Singleton);
            _container.Register<LibraryService>(Lifestyle.Singleton);
            _container.Register<ImageService>(Lifestyle.Singleton);
            _container.Register<IHardwareService>(HardwareFactory.GetHardwareService, Lifestyle.Singleton);
            LoadPlugins();
            LoadNodes();
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
            var containerNodePath = "/root/AutoloadScene";
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
                field.SetValue(__instance, obj);
            }
        }

        private void LoadPlugins() {
            List<Assembly> sources = new List<Assembly>();
        
            if (System.IO.Directory.Exists(ProjectSettings.GlobalizePath("plugins"))) {
                foreach (var dll in System.IO.Directory.GetFiles(ProjectSettings.GlobalizePath("plugins"), "*.dll")) {
                    Assembly plugin = Assembly.LoadFrom(dll);
                    sources.Add(plugin);
                }
            }
            _container.Collection.Register<ILibrarySource>(sources);
        }

        private void LoadNodes() {
            var modalService = GetNode<ModalService>("Modal");
            _container.RegisterInstance<ModalService>(modalService);
        }
    }
}