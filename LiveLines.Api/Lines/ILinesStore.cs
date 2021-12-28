﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Lines;

public interface ILinesStore
{
    Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser);
    Task<Line> CreateLine(LoggedInUser loggedInUser, string body);
}