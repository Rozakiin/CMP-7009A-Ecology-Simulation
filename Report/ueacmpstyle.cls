%\section{Class declaration}
%    \begin{macrocode}
\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{cmpstyle}[2008/05/02 v1.0 (NLCT) 2011/09/08 v1.1 (DJS) CMP Report Style]
%    \end{macrocode}
% Base this class on article class file
%    \begin{macrocode}
\LoadClass[a4paper,11pt]{article}
%    \end{macrocode}
% Required packages
%    \begin{macrocode}
\usepackage{epsfig}
\usepackage{amssymb}
\usepackage{amsmath}
\usepackage{amsfonts}
%\RequirePackage{mathptmx}
\RequirePackage[scaled=0.9]{helvet}
\RequirePackage{courier}
\RequirePackage[textwidth=16cm,
                textheight=25cm]{geometry}
%    \end{macrocode}
% No paragraph indentation and gap between paragraphs
%    \begin{macrocode}
\setlength{\parindent}{0em}
\setlength{\parskip}{1ex}
%    \end{macrocode}
%\begin{macro}{\supervisor}
% Store supervisor's name
%    \begin{macrocode}
%\newcommand*{\supervisor}[1]{\renewcommand*{\@supervisor}{#1}}
%\newcommand*{\@supervisor}{\ClassError{cmps3p4y}{No \string\supervisor
%\space given}{}}
%    \end{macrocode}
%\end{macro}
%\begin{macro}{\maketitle}
% Title layout. (This is a modification of the definition in 
% \texttt{article.cls}.)
%    \begin{macrocode}
\renewcommand\maketitle{%
  \begingroup
    \renewcommand\thefootnote{\@fnsymbol\c@footnote}%
    \def\@makefnmark{\rlap{\@textsuperscript{\normalfont\@thefnmark}}}%
    \long\def\@makefntext##1{\parindent 1em\noindent
            \hb@xt@1.8em{%
                \hss\@textsuperscript{\normalfont\@thefnmark}}##1}%
    \if@twocolumn
      \ifnum \col@number=\@ne
        \@maketitle
      \else
        \twocolumn[\@maketitle]%
      \fi
    \else
      \newpage
      \global\@topnum\z@   % Prevents figures from going at top of page.
      \@maketitle
    \fi
  \@thanks
  \endgroup
  \setcounter{footnote}{0}%
  \global\let\thanks\relax
  \global\let\maketitle\relax
  \global\let\@maketitle\relax
  \global\let\@thanks\@empty
  \global\let\title\relax
  \global\let\author\relax
  \global\let\date\relax
  \global\let\supervisor\relax
  \global\let\and\relax
}

\renewcommand{\@maketitle}{%
{\centering
\let\footnote\thanks
{\LARGE\bfseries \@title\par}%
\vskip 1em\relax
{\large\bfseries
\begin{tabular}[t]{c}%
  \@author
\end{tabular}\par
%(Supervisor: \@supervisor)
}%
\par
}%
}
%    \end{macrocode}
%\end{macro}
%
% Modify section styles
%\begin{macro}{\section}
%    \begin{macrocode}
\renewcommand\section{%
   \@startsection{section}{1}{\z@}%
                 {-3.5ex \@plus -1ex \@minus -.2ex}%
                 {1ex \@plus 0.5ex \@minus 0.5ex}%
                 {\normalfont\Large\bfseries}}
%    \end{macrocode}
%\end{macro}
%\begin{macro}{\subsection}
%    \begin{macrocode}
\renewcommand\subsection{%
   \@startsection{subsection}{2}{\z@}%
                  {-3.25ex\@plus -1ex \@minus -.2ex}%
                  {1ex \@plus 0.2ex}%
                  {\normalfont\large\bfseries}}
%    \end{macrocode}
%\end{macro}
%\begin{macro}{\subsubsection}
%    \begin{macrocode}
\renewcommand\subsubsection{%
   \@startsection{subsubsection}{3}{\z@}%
                 {-3.25ex\@plus -1ex \@minus -.2ex}%
                 {1ex \@plus .2ex}%
                 {\normalfont\normalsize\bfseries}}
%    \end{macrocode}
%\end{macro}
