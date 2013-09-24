using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IMemberRepository
    {
        IQueryable<Member> Members { get; }
        void saveMember(Member member);
        void deleteMember(Member member);
    }
}
