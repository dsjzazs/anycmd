﻿
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Ac;
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Infra;
    using Exceptions;
    using Host;
    using Identity;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class PrivilegeSet : IPrivilegeSet
    {
        public static readonly IPrivilegeSet Empty = new PrivilegeSet(EmptyAcDomain.SingleInstance);

        private readonly List<PrivilegeBigramState> _privilegeList = new List<PrivilegeBigramState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        internal PrivilegeSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            new MessageHandler(this).Register();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _privilegeList.Clear();
                var rolePrivileges = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetPrivilegeBigrams();
                foreach (var rolePrivilege in rolePrivileges)
                {
                    var rolePrivilegeState = PrivilegeBigramState.Create(rolePrivilege);
                    _privilegeList.Add(rolePrivilegeState);
                }
                _initialized = true;
            }
        }

        public IEnumerator<PrivilegeBigramState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _privilegeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _privilegeList.GetEnumerator();
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddPrivilegeBigramCommand>,
            IHandler<PrivilegeBigramAddedEvent>,
            IHandler<UpdatePrivilegeBigramCommand>,
            IHandler<PrivilegeBigramUpdatedEvent>,
            IHandler<RemovePrivilegeBigramCommand>,
            IHandler<PrivilegeBigramRemovedEvent>,
            IHandler<OrganizationRemovingEvent>,
            IHandler<RoleRemovingEvent>,
            IHandler<FunctionRemovingEvent>,
            IHandler<MenuRemovingEvent>,
            IHandler<GroupRemovingEvent>,
            IHandler<AppSystemRemovingEvent>,
            IHandler<ResourceTypeRemovingEvent>
        {
            private readonly PrivilegeSet _set;

            internal MessageHandler(PrivilegeSet set)
            {
                this._set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                }
                messageDispatcher.Register((IHandler<AddPrivilegeBigramCommand>)this);
                messageDispatcher.Register((IHandler<PrivilegeBigramAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdatePrivilegeBigramCommand>)this);
                messageDispatcher.Register((IHandler<PrivilegeBigramUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemovePrivilegeBigramCommand>)this);
                messageDispatcher.Register((IHandler<PrivilegeBigramRemovedEvent>)this);
                messageDispatcher.Register((IHandler<OrganizationRemovingEvent>)this);
                messageDispatcher.Register((IHandler<RoleRemovingEvent>)this);
                messageDispatcher.Register((IHandler<FunctionRemovingEvent>)this);
                messageDispatcher.Register((IHandler<MenuRemovingEvent>)this);
                messageDispatcher.Register((IHandler<GroupRemovingEvent>)this);
                messageDispatcher.Register((IHandler<AppSystemRemovingEvent>)this);
                messageDispatcher.Register((IHandler<ResourceTypeRemovingEvent>)this);
            }

            public void Handle(OrganizationRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.Organization && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(RoleRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var accountPrivilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountPrivileges = accountPrivilegeRepository.AsQueryable().Where(a => a.ObjectInstanceId == message.Source.Id || a.SubjectInstanceId == message.Source.Id).ToList();
                foreach (var item in accountPrivileges)
                {
                    accountPrivilegeRepository.Remove(item);
                }
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.Role && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(FunctionRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var accountPrivilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountPrivileges = accountPrivilegeRepository.AsQueryable().Where(a => a.ObjectInstanceId == message.Source.Id || a.SubjectInstanceId == message.Source.Id).ToList();
                foreach (var item in accountPrivileges)
                {
                    accountPrivilegeRepository.Remove(item);
                }
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.Function && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(MenuRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var accountPrivilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountPrivileges = accountPrivilegeRepository.AsQueryable().Where(a => a.ObjectInstanceId == message.Source.Id || a.SubjectInstanceId == message.Source.Id).ToList();
                foreach (var item in accountPrivileges)
                {
                    accountPrivilegeRepository.Remove(item);
                }
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.Menu && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(GroupRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var accountPrivilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountPrivileges = accountPrivilegeRepository.AsQueryable().Where(a => a.ObjectInstanceId == message.Source.Id || a.SubjectInstanceId == message.Source.Id).ToList();
                foreach (var item in accountPrivileges)
                {
                    accountPrivilegeRepository.Remove(item);
                }
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.Group && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(AppSystemRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var accountPrivilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountPrivileges = accountPrivilegeRepository.AsQueryable().Where(a => a.ObjectInstanceId == message.Source.Id || a.SubjectInstanceId == message.Source.Id).ToList();
                foreach (var item in accountPrivileges)
                {
                    accountPrivilegeRepository.Remove(item);
                }
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.AppSystem && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(ResourceTypeRemovingEvent message)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var accountPrivilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountPrivileges = accountPrivilegeRepository.AsQueryable().Where(a => a.ObjectInstanceId == message.Source.Id || a.SubjectInstanceId == message.Source.Id).ToList();
                foreach (var item in accountPrivileges)
                {
                    accountPrivilegeRepository.Remove(item);
                }
                foreach (var item in privilegeList.Where(a => a.ObjectType == AcElementType.ResourceType && a.ObjectInstanceId == message.Source.Id))
                {
                    host.Handle(new RemovePrivilegeBigramCommand(item.Id));
                }
            }

            public void Handle(AddPrivilegeBigramCommand message)
            {
                this.Handle(message.Input, true);
            }

            public void Handle(PrivilegeBigramAddedEvent message)
            {
                if (message.GetType() == typeof(PrivatPrivilegeBigramAddedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(IPrivilegeBigramCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
                if (!input.Id.HasValue || input.Id.Value == Guid.Empty)
                {
                    throw new AnycmdException("意外的标识");
                }
                AcElementType subjectType;
                if (!input.SubjectType.TryParse(out subjectType))
                {
                    throw new ValidationException("非法的主体类型" + input.SubjectType);
                }
                AcElementType acObjectType;
                if (!input.ObjectType.TryParse(out acObjectType))
                {
                    throw new ValidationException("非法的客体类型" + input.ObjectType);
                }
                PrivilegeBigram entity;
                lock (this)
                {
                    switch (subjectType)
                    {
                        case AcElementType.Undefined:
                            throw new AnycmdException("意外的主体类型" + subjectType.ToString());
                        case AcElementType.Account:
                            if (!accountRepository.AsQueryable().Any(a => a.Id == input.SubjectInstanceId))
                            {
                                throw new ValidationException("给定标识的账户不存在" + input.SubjectInstanceId); ;
                            }
                            break;
                        case AcElementType.Role:
                            RoleState role;
                            if (!host.RoleSet.TryGetRole(input.SubjectInstanceId, out role))
                            {
                                throw new ValidationException("意外的角色标识" + input.SubjectInstanceId);
                            }
                            break;
                        case AcElementType.Organization:
                            OrganizationState org;
                            if (!host.OrganizationSet.TryGetOrganization(input.SubjectInstanceId, out org))
                            {
                                throw new ValidationException("意外的组织结构标识" + input.SubjectInstanceId);
                            }
                            break;
                        case AcElementType.Privilege:
                            throw new NotSupportedException();
                        default:
                            throw new AnycmdException("意外的主体类型" + subjectType.ToString());
                    }
                    switch (acObjectType)
                    {
                        case AcElementType.Undefined:
                            throw new ValidationException("意外的账户权限类型" + input.SubjectType);
                        case AcElementType.Organization:
                            OrganizationState organization;
                            if (!host.OrganizationSet.TryGetOrganization(input.ObjectInstanceId, out organization))
                            {
                                throw new ValidationException("意外的组织结构标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.Role:
                            RoleState role;
                            if (!host.RoleSet.TryGetRole(input.ObjectInstanceId, out role))
                            {
                                throw new ValidationException("意外的角色标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.Group:
                            GroupState group;
                            if (!host.GroupSet.TryGetGroup(input.ObjectInstanceId, out group))
                            {
                                throw new ValidationException("意外的工作组标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.Function:
                            FunctionState function;
                            if (!host.FunctionSet.TryGetFunction(input.ObjectInstanceId, out function))
                            {
                                throw new ValidationException("意外的功能标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.Menu:
                            MenuState menu;
                            if (!host.MenuSet.TryGetMenu(input.ObjectInstanceId, out menu))
                            {
                                throw new ValidationException("意外的菜单标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.AppSystem:
                            if (!host.AppSystemSet.ContainsAppSystem(input.ObjectInstanceId))
                            {
                                throw new ValidationException("意外的应用系统标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.ResourceType:
                            ResourceTypeState resource;
                            if (!host.ResourceTypeSet.TryGetResource(input.ObjectInstanceId, out resource))
                            {
                                throw new ValidationException("意外的资源类型标识" + input.ObjectInstanceId);
                            }
                            break;
                        case AcElementType.Privilege:
                            throw new ValidationException("暂不支持" + input.SubjectType + "类型的授权");
                        default:
                            throw new ValidationException("意外的账户权限类型" + input.SubjectType);
                    }
                    if (subjectType == AcElementType.Role && acObjectType == AcElementType.Role)
                    {
                        if (input.SubjectInstanceId == input.ObjectInstanceId)
                        {
                            throw new ValidationException("角色不能继承自己");
                        }
                        var descendantIds = new HashSet<Guid>();
                        RecDescendantRoles(input.SubjectInstanceId, descendantIds);
                        if (descendantIds.Contains(input.SubjectInstanceId))
                        {
                            throw new ValidationException("角色不能继承自己的子孙");
                        }
                    }
                    if (subjectType == AcElementType.Account && acObjectType == AcElementType.Account)
                    {
                        if (input.SubjectInstanceId == input.ObjectInstanceId)
                        {
                            throw new ValidationException("账户不能继承自己");
                        }
                        var descendantIds = new HashSet<Guid>();
                        RecDescendantAccounts(input.SubjectInstanceId, descendantIds);
                        if (descendantIds.Contains(input.SubjectInstanceId))
                        {
                            throw new ValidationException("账户不能继承自己的子孙");
                        }
                    }
                    if (subjectType == AcElementType.Account && acObjectType == AcElementType.Role)
                    {
                        var sType = UserAcSubjectType.Account.ToName();
                        var oType = AcElementType.Role.ToName();
                        var rolePrivileges = privilegeBigramRepository.AsQueryable().Where(a => a.SubjectType == sType && a.SubjectInstanceId == input.SubjectInstanceId && a.ObjectType == oType);
                        var roles = new HashSet<RoleState>();
                        RoleState role;
                        if (host.RoleSet.TryGetRole(input.ObjectInstanceId, out role))
                        {
                            roles.Add(role);
                        }
                        foreach (var item in rolePrivileges)
                        {
                            if (host.RoleSet.TryGetRole(item.ObjectInstanceId, out role))
                            {
                                roles.Add(role);
                            }
                        }
                        string msg;
                        if (!host.SsdSetSet.CheckRoles(roles, out msg))
                        {
                            throw new ValidationException(msg);
                        }
                        // TODO:应用静态职责分离
                    }
                    if (subjectType == AcElementType.Organization && acObjectType == AcElementType.Organization)
                    {
                        if (input.SubjectInstanceId == input.ObjectInstanceId)
                        {
                            throw new ValidationException("组织机构不能继承自己");
                        }
                        var descendantIds = new HashSet<Guid>();
                        RecDescendantOrganizations(input.SubjectInstanceId, descendantIds);
                        if (descendantIds.Contains(input.SubjectInstanceId))
                        {
                            throw new ValidationException("组织机构不能继承自己的子孙");
                        }
                    }
                    if (host.PrivilegeSet.Any(a => a.ObjectType == acObjectType && a.ObjectInstanceId == input.ObjectInstanceId && a.SubjectType == subjectType && a.SubjectInstanceId == input.SubjectInstanceId))
                    {
                        return;
                    }

                    entity = PrivilegeBigram.Create(input);

                    if (subjectType != AcElementType.Account && privilegeList.All(a => a.Id != entity.Id))
                    {
                        privilegeList.Add(PrivilegeBigramState.Create(entity));
                    }
                    if (isCommand)
                    {
                        try
                        {
                            privilegeBigramRepository.Add(entity);
                            privilegeBigramRepository.Context.Commit();
                        }
                        catch
                        {
                            if (subjectType != AcElementType.Account && privilegeList.Any(a => a.Id == entity.Id))
                            {
                                var item = privilegeList.First(a => a.Id == entity.Id);
                                privilegeList.Remove(item);
                            }
                            privilegeBigramRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivatPrivilegeBigramAddedEvent(entity, input));
                }
                if (subjectType == AcElementType.Role && acObjectType == AcElementType.Role)
                {
                    host.MessageDispatcher.DispatchMessage(new RoleRolePrivilegeAddedEvent(entity));
                }
            }

            private void RecDescendantRoles(Guid roleId, HashSet<Guid> parentIds)
            {
                if (parentIds == null)
                {
                    parentIds = new HashSet<Guid>();
                }
                var privilegeList = _set._privilegeList;
                foreach (var item in privilegeList.Where(a => a.SubjectType == AcElementType.Role && a.SubjectInstanceId == roleId && a.ObjectType == AcElementType.Role))
                {
                    RecDescendantRoles(item.ObjectInstanceId, parentIds);
                    parentIds.Add(item.ObjectInstanceId);
                }
            }

            private void RecDescendantOrganizations(Guid organizationId, HashSet<Guid> parentIds)
            {
                if (parentIds == null)
                {
                    parentIds = new HashSet<Guid>();
                }
                var privilegeList = _set._privilegeList;
                foreach (var item in privilegeList.Where(a => a.SubjectType == AcElementType.Organization && a.SubjectInstanceId == organizationId && a.ObjectType == AcElementType.Organization))
                {
                    RecDescendantRoles(item.ObjectInstanceId, parentIds);
                    parentIds.Add(item.ObjectInstanceId);
                }
            }

            private void RecDescendantAccounts(Guid accountId, HashSet<Guid> parentIds)
            {
                if (parentIds == null)
                {
                    parentIds = new HashSet<Guid>();
                }
                var privilegeList = _set._privilegeList;
                foreach (var item in privilegeList.Where(a => a.SubjectType == AcElementType.Account && a.SubjectInstanceId == accountId && a.ObjectType == AcElementType.Account))
                {
                    RecDescendantRoles(item.ObjectInstanceId, parentIds);
                    parentIds.Add(item.ObjectInstanceId);
                }
            }

            private class PrivatPrivilegeBigramAddedEvent : PrivilegeBigramAddedEvent
            {
                internal PrivatPrivilegeBigramAddedEvent(PrivilegeBigramBase source, IPrivilegeBigramCreateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(UpdatePrivilegeBigramCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(PrivilegeBigramUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivatePrivilegeBigramUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(IPrivilegeBigramUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var privilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                PrivilegeBigram entity = null;
                var stateChanged = false;
                lock (this)
                {
                    entity = privilegeRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException("不存在的权限记录标识" + input.Id);
                    }
                    var bkState = host.PrivilegeSet.FirstOrDefault(a => a.Id == input.Id);
                    bool isAccountSubjectType = string.Equals(UserAcSubjectType.Account.ToName(), entity.SubjectType);

                    entity.Update(input);

                    var newState = PrivilegeBigramState.Create(entity);
                    stateChanged = newState != bkState;
                    if (!isAccountSubjectType && stateChanged)
                    {
                        privilegeList.Remove(bkState);
                        privilegeList.Add(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            privilegeRepository.Update(entity);
                            privilegeRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!isAccountSubjectType && stateChanged)
                            {
                                privilegeList.Remove(newState);
                                privilegeList.Add(bkState);
                            }
                            privilegeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivatePrivilegeBigramUpdatedEvent(entity, input));
                }
            }

            private class PrivatePrivilegeBigramUpdatedEvent : PrivilegeBigramUpdatedEvent
            {
                internal PrivatePrivilegeBigramUpdatedEvent(PrivilegeBigramBase source, IPrivilegeBigramUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemovePrivilegeBigramCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(PrivilegeBigramRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivatePrivilegeBigramRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid rolePrivilegeId, bool isCommand)
            {
                var host = _set._host;
                var privilegeList = _set._privilegeList;
                var privilegeRepository = host.RetrieveRequiredService<IRepository<PrivilegeBigram>>();
                PrivilegeBigram entity;
                UserAcSubjectType subjectType;
                AcElementType acObjectType;
                lock (this)
                {
                    var bkState = host.PrivilegeSet.FirstOrDefault(a => a.Id == rolePrivilegeId);
                    entity = privilegeRepository.GetByKey(rolePrivilegeId);
                    bool isAccountSubjectType = bkState == null;
                    if (entity == null)
                    {
                        return;
                    }
                    if (!entity.SubjectType.TryParse(out subjectType))
                    {
                        throw new ValidationException("非法的主体类型" + entity.SubjectType);
                    }
                    if (!entity.ObjectType.TryParse(out acObjectType))
                    {
                        throw new ValidationException("非法的客体类型" + entity.ObjectType);
                    }
                    if (!isAccountSubjectType)
                    {
                        privilegeList.Remove(bkState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            privilegeRepository.Remove(entity);
                            privilegeRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!isAccountSubjectType)
                            {
                                privilegeList.Add(bkState);
                            }
                            privilegeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivatePrivilegeBigramRemovedEvent(entity));
                    if (subjectType == UserAcSubjectType.Role && acObjectType == AcElementType.Role)
                    {
                        host.MessageDispatcher.DispatchMessage(new RoleRolePrivilegeRemovedEvent(entity));
                    }
                }
            }

            private class PrivatePrivilegeBigramRemovedEvent : PrivilegeBigramRemovedEvent
            {
                internal PrivatePrivilegeBigramRemovedEvent(PrivilegeBigramBase source)
                    : base(source)
                {

                }
            }
        }
        #endregion
    }
}