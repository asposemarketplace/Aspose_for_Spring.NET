#region License

/*
 * Copyright 2002-2007 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System.Collections;
using System.Collections.Generic;
using Spring.Data.NHibernate.Support;

using Spring.Northwind.Domain;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;

#endregion

namespace Spring.Northwind.Dao.NHibernate
{
    [Repository]
    public class HibernateCategoryDao : HibernateDao, ICategoryDao
    {
        [Transaction(ReadOnly = true)]
        public Category Get(int categoryId)
        {
            return CurrentSession.Get<Category>(categoryId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Category> GetAll()
        {
            return GetAll<Category>();
        }

        [Transaction]
        public int Save(Category category)
        {
            return (int)CurrentSession.Save(category);
        }

        [Transaction]
        public void Update(Category category)
        {
            CurrentSession.SaveOrUpdate(category);
        }

        [Transaction]
        public void Delete(Category category)
        {
            CurrentSession.Delete(category);
        }
    }
}
