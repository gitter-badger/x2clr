﻿// Copyright (c) 2013 Jae-jun Kang
// See the file COPYING for license details.

using System;

namespace xpiler {
  /// <summary>
  /// Document file handler interface.
  /// </summary>
  public interface Handler {
    bool Handle(string path, out Document doc);
  }
}