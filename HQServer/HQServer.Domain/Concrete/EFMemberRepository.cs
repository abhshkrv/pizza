using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFMemberRepository : IMemberRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Member> Members
        {
            get { return context.Members; }
        }

        public void saveMember(Member member)
        {

        }

        public void deleteMember(Member member)
        {
            context.Members.Remove(member);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
