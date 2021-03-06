﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.DomainModel;
using NUnit.Framework;

namespace NHibernate.Test.Legacy
{
	using System.Threading.Tasks;
	[TestFixture]
	public class ABCProxyTestAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"ABCProxy.hbm.xml"}; }
		}

		[Test]
		public async Task OptionalOneToOneInCollectionAsync()
		{
			C2 c2;

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				C1 c1 = new C1();
				c2 = new C2();
				c1.C2 = c2;
				c2.C1 = c1;
				c2.C1s = new List<C1>();
				c2.C1s.Add(c1);
				c1.C2 = c2;
				await (s.SaveAsync(c2));
				await (s.SaveAsync(c1));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				c2 = (C2) await (s.GetAsync(typeof(C2), c2.Id));
				Assert.IsTrue(c2.C1s.Count == 1);
				await (s.DeleteAsync(c2.C1s[0]));
				await (s.DeleteAsync(c2));
				await (t.CommitAsync());
			}
		}

		[Test]
		public async Task SharedColumnAsync()
		{
			C1 c1;
			C2 c2;

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				c1 = new C1();
				c2 = new C2();
				c1.C2 = c2;
				c2.C1 = c1;
				await (s.SaveAsync(c1));
				await (s.SaveAsync(c2));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				IList list = await (s.CreateQuery("from B").ListAsync());
				Assert.AreEqual(2, list.Count);
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				c1 = (C1) await (s.CreateQuery("from C1").UniqueResultAsync());
				c2 = (C2) await (s.CreateQuery("from C2").UniqueResultAsync());
				Assert.AreSame(c2, c1.C2);
				Assert.AreSame(c1, c2.C1);
				Assert.IsTrue(c1.C2s.Contains(c2));
				Assert.IsTrue(c2.C1s.Contains(c1));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				c1 = (C1) await (s.GetAsync(typeof(A), c1.Id));
				c2 = (C2) await (s.GetAsync(typeof(A), c2.Id));
				Assert.AreSame(c2, c1.C2);
				Assert.AreSame(c1, c2.C1);
				Assert.IsTrue(c1.C2s.Contains(c2));
				Assert.IsTrue(c2.C1s.Contains(c1));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.DeleteAsync(c1));
				await (s.DeleteAsync(c2));
				await (t.CommitAsync());
			}
		}

		[Test]
		public async Task SubclassingAsync()
		{
			C1 c1;

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				c1 = new C1();
				D d = new D();
				d.Amount = 213.34f;
				c1.Address = "foo bar";
				c1.Count = 23432;
				c1.Name = "c1";
				c1.D = d;
				await (s.SaveAsync(c1));
				d.Id = c1.Id;
				await (s.SaveAsync(d));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				// Test won't run after this line because of proxy initalization problems
				A c1a = (A) await (s.LoadAsync(typeof(A), c1.Id));
				Assert.IsFalse(NHibernateUtil.IsInitialized(c1a));
				Assert.IsTrue(c1a.Name.Equals("c1"));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				B c1b = (B) await (s.LoadAsync(typeof(B), c1.Id));
				Assert.IsTrue(
					(c1b.Count == 23432) &&
					c1b.Name.Equals("c1")
					);
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				c1 = (C1) await (s.LoadAsync(typeof(C1), c1.Id));
				Assert.IsTrue(
					c1.Address.Equals("foo bar") &&
					(c1.Count == 23432) &&
					c1.Name.Equals("c1") &&
					c1.D.Amount > 213.3f
					);
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				A c1a = (A) await (s.LoadAsync(typeof(A), c1.Id));
				Assert.IsTrue(c1a.Name.Equals("c1"));
				c1 = (C1) await (s.LoadAsync(typeof(C1), c1.Id));
				Assert.IsTrue(
					c1.Address.Equals("foo bar") &&
					(c1.Count == 23432) &&
					c1.Name.Equals("c1") &&
					c1.D.Amount > 213.3f
					);
				B c1b = (B) await (s.LoadAsync(typeof(B), c1.Id));
				Assert.IsTrue(
					(c1b.Count == 23432) &&
					c1b.Name.Equals("c1")
					);
				Assert.IsTrue(c1a.Name.Equals("c1"));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				A c1a = (A) await (s.LoadAsync(typeof(A), c1.Id));
				Assert.IsTrue(c1a.Name.Equals("c1"));
				c1 = (C1) await (s.LoadAsync(typeof(C1), c1.Id, LockMode.Upgrade));
				Assert.IsTrue(
					c1.Address.Equals("foo bar") &&
					(c1.Count == 23432) &&
					c1.Name.Equals("c1") &&
					c1.D.Amount > 213.3f
					);
				B c1b = (B) await (s.LoadAsync(typeof(B), c1.Id, LockMode.Upgrade));
				Assert.IsTrue(
					(c1b.Count == 23432) &&
					c1b.Name.Equals("c1")
					);
				Assert.IsTrue(c1a.Name.Equals("c1"));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				A c1a = (A) await (s.LoadAsync(typeof(A), c1.Id));
				c1 = (C1) await (s.LoadAsync(typeof(C1), c1.Id));
				B c1b = (B) await (s.LoadAsync(typeof(B), c1.Id));
				Assert.IsTrue(c1a.Name.Equals("c1"));
				Assert.IsTrue(
					c1.Address.Equals("foo bar") &&
					(c1.Count == 23432) &&
					c1.Name.Equals("c1") &&
					c1.D.Amount > 213.3f
					);
				Assert.IsTrue(
					(c1b.Count == 23432) &&
					c1b.Name.Equals("c1")
					);
				Console.Out.WriteLine(await (s.DeleteAsync("from a in class A")));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.SaveAsync(new B()));
				await (s.SaveAsync(new A()));
				Assert.IsTrue((await (s.CreateQuery("from b in class B").ListAsync())).Count == 1);
				Assert.IsTrue((await (s.CreateQuery("from a in class A").ListAsync())).Count == 2);
				await (s.DeleteAsync("from a in class A"));
				await (s.DeleteAsync(c1.D));
				await (t.CommitAsync());
			}
		}

		[Test]
		public async Task SubclassMapAsync()
		{
			//Test is converted, but the original didn't check anything
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			B b = new B();
			await (s.SaveAsync(b));
			IDictionary<string, string> map = new Dictionary<string, string>();
			map.Add("3", "1");
			b.Map = map;
			await (s.FlushAsync());
			await (s.DeleteAsync(b));
			await (t.CommitAsync());
			s.Close();

			s = OpenSession();
			t = s.BeginTransaction();
			map = new Dictionary<string, string>();
			map.Add("3", "1");
			b = new B();
			b.Map = map;
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			await (s.DeleteAsync(b));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task OneToOneAsync()
		{
			A a = new A();
			E d1 = new E();
			C1 c = new C1();
			E d2 = new E();
			a.Forward = d1;
			d1.Reverse = a;
			c.Forward = d2;
			d2.Reverse = c;

			object aid;
			object d2id;

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				aid = await (s.SaveAsync(a));
				d2id = await (s.SaveAsync(d2));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				IList l;
				l = await (s.CreateQuery("from E e join fetch e.Reverse").ListAsync());
				Assert.AreEqual(2, l.Count);
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				IList l = await (s.CreateQuery("from E e").ListAsync());
				Assert.AreEqual(2, l.Count);
				E e = (E) l[0];
				Assert.AreSame(e, e.Reverse.Forward);
				e = (E) l[1];
				Assert.AreSame(e, e.Reverse.Forward);
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				a = (A) await (s.LoadAsync(typeof(A), aid));
				d2 = (E) await (s.LoadAsync(typeof(E), d2id));
				Assert.AreSame(a, a.Forward.Reverse);
				Assert.AreSame(d2, d2.Reverse.Forward);
				await (s.DeleteAsync(a));
				await (s.DeleteAsync(a.Forward));
				await (s.DeleteAsync(d2));
				await (s.DeleteAsync(d2.Reverse));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				IList l = await (s.CreateQuery("from E e").ListAsync());
				Assert.AreEqual(0, l.Count);
				await (t.CommitAsync());
			}
		}
	}
}