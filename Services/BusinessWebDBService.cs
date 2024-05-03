using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using BusinessWeb.Data;

namespace BusinessWeb
{
    public partial class BusinessWebDBService
    {
        BusinessWebDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly BusinessWebDBContext context;
        private readonly NavigationManager navigationManager;

        public BusinessWebDBService(BusinessWebDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAspNetRoleClaimsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetroleclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetroleclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetRoleClaimsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetroleclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetroleclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetRoleClaimsRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim>> GetAspNetRoleClaims(Query query = null)
        {
            var items = Context.AspNetRoleClaims.AsQueryable();

            items = items.Include(i => i.AspNetRole);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetRoleClaimsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetRoleClaimGet(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);
        partial void OnGetAspNetRoleClaimById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> GetAspNetRoleClaimById(int id)
        {
            var items = Context.AspNetRoleClaims
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.AspNetRole);
 
            OnGetAspNetRoleClaimById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetRoleClaimGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetRoleClaimCreated(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimCreated(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> CreateAspNetRoleClaim(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim aspnetroleclaim)
        {
            OnAspNetRoleClaimCreated(aspnetroleclaim);

            var existingItem = Context.AspNetRoleClaims
                              .Where(i => i.Id == aspnetroleclaim.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetRoleClaims.Add(aspnetroleclaim);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetroleclaim).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetRoleClaimCreated(aspnetroleclaim);

            return aspnetroleclaim;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> CancelAspNetRoleClaimChanges(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetRoleClaimUpdated(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimUpdated(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> UpdateAspNetRoleClaim(int id, BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim aspnetroleclaim)
        {
            OnAspNetRoleClaimUpdated(aspnetroleclaim);

            var itemToUpdate = Context.AspNetRoleClaims
                              .Where(i => i.Id == aspnetroleclaim.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetroleclaim);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetRoleClaimUpdated(aspnetroleclaim);

            return aspnetroleclaim;
        }

        partial void OnAspNetRoleClaimDeleted(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimDeleted(BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> DeleteAspNetRoleClaim(int id)
        {
            var itemToDelete = Context.AspNetRoleClaims
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetRoleClaimDeleted(itemToDelete);


            Context.AspNetRoleClaims.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetRoleClaimDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetRolesRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetRole> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetRole>> GetAspNetRoles(Query query = null)
        {
            var items = Context.AspNetRoles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetRoleGet(BusinessWeb.Models.BusinessWebDB.AspNetRole item);
        partial void OnGetAspNetRoleById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetRole> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRole> GetAspNetRoleById(string id)
        {
            var items = Context.AspNetRoles
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAspNetRoleById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetRoleCreated(BusinessWeb.Models.BusinessWebDB.AspNetRole item);
        partial void OnAfterAspNetRoleCreated(BusinessWeb.Models.BusinessWebDB.AspNetRole item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRole> CreateAspNetRole(BusinessWeb.Models.BusinessWebDB.AspNetRole aspnetrole)
        {
            OnAspNetRoleCreated(aspnetrole);

            var existingItem = Context.AspNetRoles
                              .Where(i => i.Id == aspnetrole.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetRoles.Add(aspnetrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetrole).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetRoleCreated(aspnetrole);

            return aspnetrole;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRole> CancelAspNetRoleChanges(BusinessWeb.Models.BusinessWebDB.AspNetRole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetRoleUpdated(BusinessWeb.Models.BusinessWebDB.AspNetRole item);
        partial void OnAfterAspNetRoleUpdated(BusinessWeb.Models.BusinessWebDB.AspNetRole item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRole> UpdateAspNetRole(string id, BusinessWeb.Models.BusinessWebDB.AspNetRole aspnetrole)
        {
            OnAspNetRoleUpdated(aspnetrole);

            var itemToUpdate = Context.AspNetRoles
                              .Where(i => i.Id == aspnetrole.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetRoleUpdated(aspnetrole);

            return aspnetrole;
        }

        partial void OnAspNetRoleDeleted(BusinessWeb.Models.BusinessWebDB.AspNetRole item);
        partial void OnAfterAspNetRoleDeleted(BusinessWeb.Models.BusinessWebDB.AspNetRole item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetRole> DeleteAspNetRole(string id)
        {
            var itemToDelete = Context.AspNetRoles
                              .Where(i => i.Id == id)
                              .Include(i => i.AspNetRoleClaims)
                              .Include(i => i.AspNetUserRoles)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetRoleDeleted(itemToDelete);


            Context.AspNetRoles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserClaimsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetuserclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetuserclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserClaimsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetuserclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetuserclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserClaimsRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim>> GetAspNetUserClaims(Query query = null)
        {
            var items = Context.AspNetUserClaims.AsQueryable();

            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserClaimsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserClaimGet(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);
        partial void OnGetAspNetUserClaimById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> GetAspNetUserClaimById(int id)
        {
            var items = Context.AspNetUserClaims
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserClaimById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserClaimGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserClaimCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);
        partial void OnAfterAspNetUserClaimCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> CreateAspNetUserClaim(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim aspnetuserclaim)
        {
            OnAspNetUserClaimCreated(aspnetuserclaim);

            var existingItem = Context.AspNetUserClaims
                              .Where(i => i.Id == aspnetuserclaim.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserClaims.Add(aspnetuserclaim);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuserclaim).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserClaimCreated(aspnetuserclaim);

            return aspnetuserclaim;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> CancelAspNetUserClaimChanges(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserClaimUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);
        partial void OnAfterAspNetUserClaimUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> UpdateAspNetUserClaim(int id, BusinessWeb.Models.BusinessWebDB.AspNetUserClaim aspnetuserclaim)
        {
            OnAspNetUserClaimUpdated(aspnetuserclaim);

            var itemToUpdate = Context.AspNetUserClaims
                              .Where(i => i.Id == aspnetuserclaim.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuserclaim);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserClaimUpdated(aspnetuserclaim);

            return aspnetuserclaim;
        }

        partial void OnAspNetUserClaimDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);
        partial void OnAfterAspNetUserClaimDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserClaim item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> DeleteAspNetUserClaim(int id)
        {
            var itemToDelete = Context.AspNetUserClaims
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserClaimDeleted(itemToDelete);


            Context.AspNetUserClaims.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserClaimDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserLoginsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetuserlogins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetuserlogins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserLoginsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetuserlogins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetuserlogins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserLoginsRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin>> GetAspNetUserLogins(Query query = null)
        {
            var items = Context.AspNetUserLogins.AsQueryable();

            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserLoginsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserLoginGet(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);
        partial void OnGetAspNetUserLoginByLoginProviderAndProviderKey(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> GetAspNetUserLoginByLoginProviderAndProviderKey(string loginprovider, string providerkey)
        {
            var items = Context.AspNetUserLogins
                              .AsNoTracking()
                              .Where(i => i.LoginProvider == loginprovider && i.ProviderKey == providerkey);

            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserLoginByLoginProviderAndProviderKey(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserLoginGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserLoginCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> CreateAspNetUserLogin(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin aspnetuserlogin)
        {
            OnAspNetUserLoginCreated(aspnetuserlogin);

            var existingItem = Context.AspNetUserLogins
                              .Where(i => i.LoginProvider == aspnetuserlogin.LoginProvider && i.ProviderKey == aspnetuserlogin.ProviderKey)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserLogins.Add(aspnetuserlogin);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuserlogin).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserLoginCreated(aspnetuserlogin);

            return aspnetuserlogin;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> CancelAspNetUserLoginChanges(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserLoginUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> UpdateAspNetUserLogin(string loginprovider, string providerkey, BusinessWeb.Models.BusinessWebDB.AspNetUserLogin aspnetuserlogin)
        {
            OnAspNetUserLoginUpdated(aspnetuserlogin);

            var itemToUpdate = Context.AspNetUserLogins
                              .Where(i => i.LoginProvider == aspnetuserlogin.LoginProvider && i.ProviderKey == aspnetuserlogin.ProviderKey)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuserlogin);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserLoginUpdated(aspnetuserlogin);

            return aspnetuserlogin;
        }

        partial void OnAspNetUserLoginDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserLogin item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> DeleteAspNetUserLogin(string loginprovider, string providerkey)
        {
            var itemToDelete = Context.AspNetUserLogins
                              .Where(i => i.LoginProvider == loginprovider && i.ProviderKey == providerkey)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserLoginDeleted(itemToDelete);


            Context.AspNetUserLogins.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserLoginDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetuserroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetuserroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetuserroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetuserroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserRolesRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserRole>> GetAspNetUserRoles(Query query = null)
        {
            var items = Context.AspNetUserRoles.AsQueryable();

            items = items.Include(i => i.AspNetRole);
            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserRoleGet(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);
        partial void OnGetAspNetUserRoleByUserIdAndRoleId(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> GetAspNetUserRoleByUserIdAndRoleId(string userid, string roleid)
        {
            var items = Context.AspNetUserRoles
                              .AsNoTracking()
                              .Where(i => i.UserId == userid && i.RoleId == roleid);

            items = items.Include(i => i.AspNetRole);
            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserRoleByUserIdAndRoleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserRoleCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> CreateAspNetUserRole(BusinessWeb.Models.BusinessWebDB.AspNetUserRole aspnetuserrole)
        {
            OnAspNetUserRoleCreated(aspnetuserrole);

            var existingItem = Context.AspNetUserRoles
                              .Where(i => i.UserId == aspnetuserrole.UserId && i.RoleId == aspnetuserrole.RoleId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserRoles.Add(aspnetuserrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuserrole).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserRoleCreated(aspnetuserrole);

            return aspnetuserrole;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> CancelAspNetUserRoleChanges(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserRoleUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> UpdateAspNetUserRole(string userid, string roleid, BusinessWeb.Models.BusinessWebDB.AspNetUserRole aspnetuserrole)
        {
            OnAspNetUserRoleUpdated(aspnetuserrole);

            var itemToUpdate = Context.AspNetUserRoles
                              .Where(i => i.UserId == aspnetuserrole.UserId && i.RoleId == aspnetuserrole.RoleId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuserrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserRoleUpdated(aspnetuserrole);

            return aspnetuserrole;
        }

        partial void OnAspNetUserRoleDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserRole item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> DeleteAspNetUserRole(string userid, string roleid)
        {
            var itemToDelete = Context.AspNetUserRoles
                              .Where(i => i.UserId == userid && i.RoleId == roleid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserRoleDeleted(itemToDelete);


            Context.AspNetUserRoles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUsersRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUser> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUser>> GetAspNetUsers(Query query = null)
        {
            var items = Context.AspNetUsers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserGet(BusinessWeb.Models.BusinessWebDB.AspNetUser item);
        partial void OnGetAspNetUserById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUser> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUser> GetAspNetUserById(string id)
        {
            var items = Context.AspNetUsers
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAspNetUserById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserCreated(BusinessWeb.Models.BusinessWebDB.AspNetUser item);
        partial void OnAfterAspNetUserCreated(BusinessWeb.Models.BusinessWebDB.AspNetUser item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUser> CreateAspNetUser(BusinessWeb.Models.BusinessWebDB.AspNetUser aspnetuser)
        {
            OnAspNetUserCreated(aspnetuser);

            var existingItem = Context.AspNetUsers
                              .Where(i => i.Id == aspnetuser.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUsers.Add(aspnetuser);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuser).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserCreated(aspnetuser);

            return aspnetuser;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUser> CancelAspNetUserChanges(BusinessWeb.Models.BusinessWebDB.AspNetUser item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUser item);
        partial void OnAfterAspNetUserUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUser item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUser> UpdateAspNetUser(string id, BusinessWeb.Models.BusinessWebDB.AspNetUser aspnetuser)
        {
            OnAspNetUserUpdated(aspnetuser);

            var itemToUpdate = Context.AspNetUsers
                              .Where(i => i.Id == aspnetuser.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuser);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserUpdated(aspnetuser);

            return aspnetuser;
        }

        partial void OnAspNetUserDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUser item);
        partial void OnAfterAspNetUserDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUser item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUser> DeleteAspNetUser(string id)
        {
            var itemToDelete = Context.AspNetUsers
                              .Where(i => i.Id == id)
                              .Include(i => i.AspNetUserClaims)
                              .Include(i => i.AspNetUserLogins)
                              .Include(i => i.AspNetUserRoles)
                              .Include(i => i.AspNetUserTokens)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserDeleted(itemToDelete);


            Context.AspNetUsers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserTokensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetusertokens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetusertokens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserTokensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/aspnetusertokens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/aspnetusertokens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserTokensRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserToken>> GetAspNetUserTokens(Query query = null)
        {
            var items = Context.AspNetUserTokens.AsQueryable();

            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserTokensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserTokenGet(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);
        partial void OnGetAspNetUserTokenByUserIdAndLoginProviderAndName(ref IQueryable<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> GetAspNetUserTokenByUserIdAndLoginProviderAndName(string userid, string loginprovider, string name)
        {
            var items = Context.AspNetUserTokens
                              .AsNoTracking()
                              .Where(i => i.UserId == userid && i.LoginProvider == loginprovider && i.Name == name);

            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserTokenByUserIdAndLoginProviderAndName(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserTokenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserTokenCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenCreated(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> CreateAspNetUserToken(BusinessWeb.Models.BusinessWebDB.AspNetUserToken aspnetusertoken)
        {
            OnAspNetUserTokenCreated(aspnetusertoken);

            var existingItem = Context.AspNetUserTokens
                              .Where(i => i.UserId == aspnetusertoken.UserId && i.LoginProvider == aspnetusertoken.LoginProvider && i.Name == aspnetusertoken.Name)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserTokens.Add(aspnetusertoken);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetusertoken).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserTokenCreated(aspnetusertoken);

            return aspnetusertoken;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> CancelAspNetUserTokenChanges(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserTokenUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenUpdated(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> UpdateAspNetUserToken(string userid, string loginprovider, string name, BusinessWeb.Models.BusinessWebDB.AspNetUserToken aspnetusertoken)
        {
            OnAspNetUserTokenUpdated(aspnetusertoken);

            var itemToUpdate = Context.AspNetUserTokens
                              .Where(i => i.UserId == aspnetusertoken.UserId && i.LoginProvider == aspnetusertoken.LoginProvider && i.Name == aspnetusertoken.Name)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetusertoken);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserTokenUpdated(aspnetusertoken);

            return aspnetusertoken;
        }

        partial void OnAspNetUserTokenDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenDeleted(BusinessWeb.Models.BusinessWebDB.AspNetUserToken item);

        public async Task<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> DeleteAspNetUserToken(string userid, string loginprovider, string name)
        {
            var itemToDelete = Context.AspNetUserTokens
                              .Where(i => i.UserId == userid && i.LoginProvider == loginprovider && i.Name == name)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserTokenDeleted(itemToDelete);


            Context.AspNetUserTokens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserTokenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTLicensesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/tlicenses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/tlicenses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTLicensesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/tlicenses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/tlicenses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTLicensesRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.TLicense> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.TLicense>> GetTLicenses(Query query = null)
        {
            var items = Context.TLicenses.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTLicensesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTLicenseGet(BusinessWeb.Models.BusinessWebDB.TLicense item);
        partial void OnGetTLicenseById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.TLicense> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.TLicense> GetTLicenseById(int id)
        {
            var items = Context.TLicenses
                              .AsNoTracking()
                              .Where(i => i.id == id);

 
            OnGetTLicenseById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTLicenseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTLicenseCreated(BusinessWeb.Models.BusinessWebDB.TLicense item);
        partial void OnAfterTLicenseCreated(BusinessWeb.Models.BusinessWebDB.TLicense item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TLicense> CreateTLicense(BusinessWeb.Models.BusinessWebDB.TLicense tlicense)
        {
            OnTLicenseCreated(tlicense);

            var existingItem = Context.TLicenses
                              .Where(i => i.id == tlicense.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TLicenses.Add(tlicense);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tlicense).State = EntityState.Detached;
                throw;
            }

            OnAfterTLicenseCreated(tlicense);

            return tlicense;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.TLicense> CancelTLicenseChanges(BusinessWeb.Models.BusinessWebDB.TLicense item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTLicenseUpdated(BusinessWeb.Models.BusinessWebDB.TLicense item);
        partial void OnAfterTLicenseUpdated(BusinessWeb.Models.BusinessWebDB.TLicense item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TLicense> UpdateTLicense(int id, BusinessWeb.Models.BusinessWebDB.TLicense tlicense)
        {
            OnTLicenseUpdated(tlicense);

            var itemToUpdate = Context.TLicenses
                              .Where(i => i.id == tlicense.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tlicense);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTLicenseUpdated(tlicense);

            return tlicense;
        }

        partial void OnTLicenseDeleted(BusinessWeb.Models.BusinessWebDB.TLicense item);
        partial void OnAfterTLicenseDeleted(BusinessWeb.Models.BusinessWebDB.TLicense item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TLicense> DeleteTLicense(int id)
        {
            var itemToDelete = Context.TLicenses
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTLicenseDeleted(itemToDelete);


            Context.TLicenses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTLicenseDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTSocietesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/tsocietes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/tsocietes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTSocietesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/tsocietes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/tsocietes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTSocietesRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.TSociete> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.TSociete>> GetTSocietes(Query query = null)
        {
            var items = Context.TSocietes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTSocietesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTSocieteGet(BusinessWeb.Models.BusinessWebDB.TSociete item);
        partial void OnGetTSocieteById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.TSociete> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.TSociete> GetTSocieteById(int id)
        {
            var items = Context.TSocietes
                              .AsNoTracking()
                              .Where(i => i.id == id);

 
            OnGetTSocieteById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTSocieteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTSocieteCreated(BusinessWeb.Models.BusinessWebDB.TSociete item);
        partial void OnAfterTSocieteCreated(BusinessWeb.Models.BusinessWebDB.TSociete item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TSociete> CreateTSociete(BusinessWeb.Models.BusinessWebDB.TSociete tsociete)
        {
            OnTSocieteCreated(tsociete);

            var existingItem = Context.TSocietes
                              .Where(i => i.id == tsociete.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TSocietes.Add(tsociete);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tsociete).State = EntityState.Detached;
                throw;
            }

            OnAfterTSocieteCreated(tsociete);

            return tsociete;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.TSociete> CancelTSocieteChanges(BusinessWeb.Models.BusinessWebDB.TSociete item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTSocieteUpdated(BusinessWeb.Models.BusinessWebDB.TSociete item);
        partial void OnAfterTSocieteUpdated(BusinessWeb.Models.BusinessWebDB.TSociete item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TSociete> UpdateTSociete(int id, BusinessWeb.Models.BusinessWebDB.TSociete tsociete)
        {
            OnTSocieteUpdated(tsociete);

            var itemToUpdate = Context.TSocietes
                              .Where(i => i.id == tsociete.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tsociete);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTSocieteUpdated(tsociete);

            return tsociete;
        }

        partial void OnTSocieteDeleted(BusinessWeb.Models.BusinessWebDB.TSociete item);
        partial void OnAfterTSocieteDeleted(BusinessWeb.Models.BusinessWebDB.TSociete item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TSociete> DeleteTSociete(int id)
        {
            var itemToDelete = Context.TSocietes
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTSocieteDeleted(itemToDelete);


            Context.TSocietes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTSocieteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTAuthorizesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/tauthorizes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/tauthorizes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTAuthorizesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/BusinessWebdb/tauthorizes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/BusinessWebdb/tauthorizes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTAuthorizesRead(ref IQueryable<BusinessWeb.Models.BusinessWebDB.TAuthorize> items);

        public async Task<IQueryable<BusinessWeb.Models.BusinessWebDB.TAuthorize>> GetTAuthorizes(Query query = null)
        {
            var items = Context.TAuthorizes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTAuthorizesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTAuthorizeGet(BusinessWeb.Models.BusinessWebDB.TAuthorize item);
        partial void OnGetTAuthorizeById(ref IQueryable<BusinessWeb.Models.BusinessWebDB.TAuthorize> items);


        public async Task<BusinessWeb.Models.BusinessWebDB.TAuthorize> GetTAuthorizeById(int id)
        {
            var items = Context.TAuthorizes
                              .AsNoTracking()
                              .Where(i => i.id == id);

 
            OnGetTAuthorizeById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTAuthorizeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTAuthorizeCreated(BusinessWeb.Models.BusinessWebDB.TAuthorize item);
        partial void OnAfterTAuthorizeCreated(BusinessWeb.Models.BusinessWebDB.TAuthorize item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TAuthorize> CreateTAuthorize(BusinessWeb.Models.BusinessWebDB.TAuthorize tauthorize)
        {
            OnTAuthorizeCreated(tauthorize);

            var existingItem = Context.TAuthorizes
                              .Where(i => i.id == tauthorize.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TAuthorizes.Add(tauthorize);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tauthorize).State = EntityState.Detached;
                throw;
            }

            OnAfterTAuthorizeCreated(tauthorize);

            return tauthorize;
        }

        public async Task<BusinessWeb.Models.BusinessWebDB.TAuthorize> CancelTAuthorizeChanges(BusinessWeb.Models.BusinessWebDB.TAuthorize item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTAuthorizeUpdated(BusinessWeb.Models.BusinessWebDB.TAuthorize item);
        partial void OnAfterTAuthorizeUpdated(BusinessWeb.Models.BusinessWebDB.TAuthorize item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TAuthorize> UpdateTAuthorize(int id, BusinessWeb.Models.BusinessWebDB.TAuthorize tauthorize)
        {
            OnTAuthorizeUpdated(tauthorize);

            var itemToUpdate = Context.TAuthorizes
                              .Where(i => i.id == tauthorize.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tauthorize);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTAuthorizeUpdated(tauthorize);

            return tauthorize;
        }

        partial void OnTAuthorizeDeleted(BusinessWeb.Models.BusinessWebDB.TAuthorize item);
        partial void OnAfterTAuthorizeDeleted(BusinessWeb.Models.BusinessWebDB.TAuthorize item);

        public async Task<BusinessWeb.Models.BusinessWebDB.TAuthorize> DeleteTAuthorize(int id)
        {
            var itemToDelete = Context.TAuthorizes
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTAuthorizeDeleted(itemToDelete);


            Context.TAuthorizes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTAuthorizeDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}