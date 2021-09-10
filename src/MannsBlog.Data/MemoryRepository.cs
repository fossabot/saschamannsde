// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MannsBlog.Data
{
  public class MemoryRepository : BaseRepository, IMannsRepository
  {
    public void AddStory(BlogStory story)
    {
      story.Id = Stories.Max(s => s.Id) + 1;
      Stories.Add(story);
    }

    public async Task<bool> DeleteStory(string postid)
    {
      var story = await GetStory(int.Parse(postid));
      if (story != null)
      {
        Stories.Remove(story);
      }

      return story != null;
    }

    public Task<IEnumerable<string>> GetCategories()
    {
      var cats = Stories
                .Select(c => c.Categories.Split(','))
                .ToList();

      var result = new List<string>();
      foreach (var s in cats) result.AddRange(s);

      return Task.FromResult(result.Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s).Distinct());
    }

    public Task<BlogResult> GetStories(int pageSize = 10, int page = 1)
    {
      return Task.FromResult(new BlogResult()
      {
        CurrentPage = page,
        TotalResults = Stories.Count(),
        TotalPages = CalculatePages(Stories.Count(), pageSize),
        Stories = Stories.Skip((page - 1) * pageSize).Take(pageSize),
      });
    }

    public Task<BlogResult> GetStoriesByTerm(string term, int pageSize, int page)
    {
      var lowerTerm = term.ToLowerInvariant();
      var totalCount = Stories.Count(s => s.Body.ToLowerInvariant().Contains(lowerTerm) ||
                                          s.Categories.ToLowerInvariant().Contains(lowerTerm) ||
                                          s.Title.ToLowerInvariant().Contains(lowerTerm));

      return Task.FromResult(new BlogResult()
      {
        CurrentPage = page,
        TotalResults = totalCount,
        TotalPages = CalculatePages(totalCount, pageSize),
        Stories = Stories
        .Where(s => s.Body.ToLowerInvariant().Contains(lowerTerm) ||
                    s.Categories.ToLowerInvariant().Contains(lowerTerm) ||
                    s.Title.ToLowerInvariant().Contains(lowerTerm))
        .Skip((page - 1) * pageSize).Take(pageSize)
      });
    }

    public Task<BlogStory> GetStory(string slug)
    {
      return Task.FromResult(Stories.FirstOrDefault(s => s.Slug == slug));
    }

    public Task<BlogStory> GetStory(int id)
    {
      return Task.FromResult(Stories.FirstOrDefault(s => s.Id == id));
    }

    public Task<BlogResult> GetStoriesByTag(string tag, int pageSize, int page)
    {
      var lowerTag = tag.ToLowerInvariant();
      var totalCount = Stories.Count(s => s.Categories.ToLowerInvariant().Split(',').Contains(lowerTag));

      return Task.FromResult(new BlogResult()
      {
        CurrentPage = page,
        TotalResults = totalCount,
        TotalPages = CalculatePages(totalCount, pageSize),
        Stories = Stories
        .Where(s => s.Categories.ToLowerInvariant().Split(',').Contains(lowerTag))
        .Skip((page - 1) * pageSize).Take(pageSize)
      });
    }

    public Task<bool> SaveAllAsync()
    {
      // NOOP
      return Task.FromResult(true);
    }

        public static readonly List<BlogStory> Stories = new List<BlogStory>()
    {
      new BlogStory()
      {
        Id = 1,
        IsPublished = true,
        Title = "MultiMarkdown für Ubuntu fixen",
        Slug = "2017/1/17/MultiMarkdown_fuer_Ubuntu_fixen",
        Categories = "Opensource,Linux,de-DE",
        DatePublished = new DateTime(2017, 1, 17),
        Body = @"<p>Wie <a href=""https://www.literatureandlatte.com/forum/viewtopic.php?f=33&amp;t=32666"" hreflang=""de"">hier</a> berichtet, scheint die Ubuntu Version von MultiMarkdown defekt zu sein.<br> Als Resultat daraus lassen sich mit Scrivener 1.9.0.1 (der aktuellen Linux-Beta) keine Latex Dokumente erstellen, da der Kompilierungsvorgang mit einem unbekannten Fehler abbricht.</p><p>Mit den folgenden Schritten habe ich die Funktion wieder zum laufen gebracht:</p><ol><li><pre><code>sudo apt-get purge libtext-multimarkdown-perl</pre></code></li><li><pre><code>sudo apt-get install cmake checkinstall</pre></code></li><li><pre><code>git clone https://github.com/fletcher/MultiMarkdown-5.git</pre></code></li><li><pre><code>cd into the checked out repository</pre></code></li><li><pre><code>./link_git_modules</pre></code></li><li><pre><code>./update_git_modules</pre></code></li><li><pre><code>make</pre></code></li><li><pre><code>cd build</pre></code></li><li><pre><code>make</pre></code></li><li><pre><code>make test</pre></code></li><li><pre><code>checkinstall</pre></code></li></ol><p>Beim vorletzten Kommando sollte genau ein Test fehlschlagen. Infos zu Checkinstall gibt es <a title=""Checkinstall"" href=""https://checkinstall.izto.org/docs.php"" hreflang=""de"">hier</a>. Mehr zur Scrievener Latex-Konfiguration gibt es <a title=""Abnormaldata"" href=""https://abnormaldata.wordpress.com/2015/01/14/configuring-scrivener-latex/"" hreflang=""de"">hier</a>.</p>"
      },
      new BlogStory()
      {
        Id = 2,
        IsPublished = true,
        Title = "Learning C Sharp",
        Slug = "2017/1/30/Learning_CSharp",
        Categories = "Development,DotNetCore,C-Sharp,en-US",
        DatePublished = new DateTime(2017, 1, 30),
        Body = @"<div class=""itemcontent""><p>Actually i’m learning C#, and i found two good starting points:</p><ul><li><a title=""Tutorial"" href=""https://scottlilly.com/learn-c-by-building-a-simple-rpg-index"" hreflang=""en"">This is a good tutorial which guides you the 20% what you are using 80% of time</a></li><li><a title = ""Highscore"" href= ""https://www.highscore.de/csharp/einfuehrung/index.html"" hreflang= ""de""> This one is a good introduction in C# (written in german) </a></li></ul></div>"
      },
      new BlogStory()
      {
        Id = 3,
        IsPublished = true,
        Title = "Fixing Ubuntu versions of MultiMarkdown",
        Slug = "2017/1/30/Fixing_Ubuntu_versions_of_MultiMarkdown",
        Categories = "Opensource,Linux,en-US",
        DatePublished = new DateTime(2017, 1, 30),
        Body = @"<div class=""itemcontent""><p>As <a title=""Literatur and Latte"" href=""https://www.literatureandlatte.com/forum/viewtopic.php?f=33&amp;t=32666"" hreflang=""en"">here</a> explained, that the Ubuntu versions of Multimarkdown are broken.<br />The result is, that you can't export Scrievener documents into Latex documents, because the compiling process breaks with a unknown error.</p><p>With that steps i have fixed it, so i can export to Latex:</p><ul><li><pre><code>sudo apt - get purge libtext - multimarkdown - perl</pre></code></li><li><pre><code>sudo apt - get install cmake checkinstall</pre></code></li><li><pre><code>git clone https://github.com/fletcher/MultiMarkdown-5.git </pre></code></li><li><pre><code> cd into the checked out repository </pre></code></li><li><pre><code>./ link_git_modules</pre></code> </li><li><pre><code>./ update_git_modules</pre></code> </li><li><pre><code> make </pre></code></li><li><pre><code> cd build </pre></code></li><li><pre><code> make</pre></code></li><li><pre><code> make test </pre></code></li><li><pre><code> checkinstall </pre></code></li></ul><p> The 'make test' command should fail with one test.Additional documents about checkinstall you can get <a title = ""Checkinstall"" href = ""https://checkinstall.izto.org/docs.php"" hreflang = ""en"" > there </a>.More about the Scrievener Latex - Configuration is available <a title = ""Abnormaldata"" href = ""https://abnormaldata.wordpress.com/2015/01/14/configuring-scrivener-latex/"" hreflang = ""en"" > there </a>.</p></div> "
      },
      new BlogStory()
      {
        Id = 4,
        IsPublished = true,
        Title = "publican available for openSUSE",
        Slug = "2017/4/6/publican_available_for_openSUSE",
        Categories = "Opensource,Linux,en-US",
        DatePublished = new DateTime(2017, 4, 6),
        Body = @"<div class=""itemcontent""><p>I'm pleased to announce publican for openSUSE.</p><p>Publican is a DocBook publication system, not just a DocBook processing tool. As well as ensuring your DocBook XML is valid, publican works to ensure your XML is up to publishable standard.<br> Because upstream depreached FOP support and prefers wkhtmltopdf, this version comes without FOP package dependencies. If you want to use FOP, then deinstall wkhtmltopdf. After the next start publican will use FOP again.</p><p>More on: <a href=""https://sourceware.org/publican/en-US/index.html"" target=""_blank"" rel=""noopener"" hreflang=""en"">https://sourceware.org/publican/en-US/index.html</a>.</p><p>How to install:</p><ol><li><pre><code>sudo zypper ar https://download.opensuse.org/repositories/Documentation:/Publican/openSUSE_Leap_42.2 </pre></code> (for openSUSE Leap 42.2) or<br><pre><code>sudo zypper ar https://download.opensuse.org/repositories/Documentation:/Publican/openSUSE_Tumbleweed </pre></code> (for openSUSE Tumbleweed)</li><li><pre><code>sudo zypper in publican</pre></code></li></ol><p>On <a href=""https://build.opensuse.org/project/show/Documentation:Publican"" target=""_blank"" rel=""noopener"" hreflang=""en"">https://build.opensuse.org/project/show/Documentation:Publican</a> you can find more packages, which names beginning with ""publican-"". That are brands for publican. You can use that brands, or can use it as base for your own brand.</p><p>Place Bugreports <a href=""https://bugzilla.opensuse.org/enter_bug.cgi?classification=7340&amp;product=openSUSE.org&amp;component=3rd%20party%20software&amp;assigned_to=saigkill@opensuse.org&amp;short_desc=Documentation:Publican/publican:%20Bug"" target=""_blank"" rel=""noopener"" hreflang=""en"">there</a>.</p></div>"
      },
      new BlogStory()
      {
        Id = 5,
        IsPublished = true,
        Title = "ignore-me 0.1.0 released",
        Slug = "2017/10/6/ignore-me_0.1.0_released",
        Categories = "Linux, Development, Opensource, en-US",
        DatePublished = new DateTime(2017, 10, 6),
        Body = @"<p>I'm pleased to announce the successor of the bzrmk package: ignore-me.</p><p>After publishing the bzrmk package some people have asked, if i can provide a package which supports more version controlling systems. So i worked on ignore-me, which currently supports BZR, GIT, SVN and HG. A experimental support of CVS is also included. I don't know exactly if this really works on a CVS repository. So please let me know.</p><p>On <a title=""How to get it"" href=""https://github.com/saigkill/ignore-me"" target=""_blank"" rel=""noopener"">https://github.com/saigkill/ignore-me</a> you can see, how to get the package. That faq explains, how to use it: <a title=""How to use"" href=""https://github.com/saigkill/ignore-me/wiki"" target=""_blank"" rel=""noopener"">https://github.com/saigkill/ignore-me/wiki</a> .</p>"
      },
      new BlogStory()
      {
        Id = 6,
        IsPublished = true,
        Title = "Copy multiple elements to the clipboard with Windows 10",
        Slug = "2018/5/27/Copy_multiple_elements_to_the_clipboard_with_Windows_10",
        Categories = "Windows, Insider, en-US",
        DatePublished = new DateTime(2018, 5, 27),
        Body = @"<h2>Saving Time in Windows 10</h2><p>Many people knowing the problem. You're working an one app, and you need some text fragments or pictures from another app. Before Windows 10&nbsp; you have to copy and paste them many times. But now you can copy multiple fragments and pictures in one time. But how to do it?</p><p>First of all you have have to register yourself as Microsoft Insider as explained<a href=""https://insider.windows.com/"" target=""_blank"" rel=""noopener""> there</a>. Then you can follow the next steps.</p><h2>How to use the new Windows 10 Clipboard?</h2><ol><li>First click inside your Windows 10 on Start &gt; Settings &gt; System &gt; Clipboard and activate Save more Elements</li><li>Then start the app, where you want to copy anything.</li><li>Now mark a element and copy it with STRG+C or with the copy command.</li><li>Do this with all needed Fragments.</li><li>Now go to the app, where the stuff should be placed.</li><li>Use the Windows-Logo-Key+V.Now you see all copied content in a small box.Just click on one element, and it's pasted into your target app.</li></ol><p>More on:&nbsp;<a href = ""https://aka.ms/AA1a313"" target=""_blank"" rel=""noopener"">https://aka.ms/AA1a313</a></p><p>Enjoy it...</p><p>&nbsp;</p>"
      },
      new BlogStory()
      {
        Id = 7,
        IsPublished = true,
        Title = "Scrum im Einsatz",
        Slug = "2018/7/6/Scrum_im_Einsatz",
        Categories = "Development, de-DE",
        DatePublished = new DateTime(2018, 7, 6),
        Body = @"<p>Wir leben heute in einer schnellebigen Zeit. Einer Zeit in der morgen schon etwas, was heute noch für richtig und wichtig befunden wurde, nicht mehr gilt. Vor noch wenigen Dekaden war auch die Softwareentwicklung starren Strukturen unterworfen. Der Kunde/Stakeholder trat mit einem Softwarewunsch an eine Firma heran. Diese setzte sich mit Lasten- und Pflichtenheften auseinander, plante das komplette Projekt durch, nur um am Ende festzustellen, dass es nicht den Erwartungen des Stakeholders entspricht.</p><p>Um den stetigen Änderungen entgegenzutreten wurden agile Vorgehensmodelle entwickelt, die sich eben diesen Änderungen anpassen können. Eines dieser möglichen Vorgehensmodelle ist SCRUM. Im Gegensatz zu zB Kanban, wurde SCRUM speziell für die Softwaretechnik entworfen und betrifft das Projekt- und Produktmanagement im besonderen.</p><h3>Was ist Scrum?</h3><p>Was ist SCRUM genau? Es ist ein Vorgehensmodell, also eine Methode, keine Software. Auch wenn es spezielle Software für den Einsatz gibt (Projekt- oder Ticketsysteme), so ist nicht zwingend ein solches Programm einzusetzen.</p><p>SCRUM hat 4 Leizsätze:</p><ul><li>Individuen und Interaktionen sind wichtiger als Prozesse und Werkzeuge</li><li>Funktionierende Software ist wichtiger als umfassende Dokumentation</li><li>Zusammenarbeit mit dem Auftraggeber ist wichtiger als die Vertragsverhandlung</li><li>Reagieren auf Veränderung ist wichtiger als das Befolgen eines Plans.</li></ul><p>Auch ergänzende 12 Grundprinzipien wurden dazu erstellt:</p><ul><li>Unsere höchste Priorität ist es, den Kunden durch frühe und kontinuierliche Auslieferung wertvoller Software zufrieden zu stellen</li><li>Heiße Anforderungsänderungen selbst spät in der Entwicklung willkommen. Agile Prozesse nutzen Veränderungen zum Wettbewerbsvorteil des Kunden</li><li>Liefere funktionierende Software regelmäßig innerhalb weniger Wochen oder Monate und bevorzuge dabei die kürzere Zeitspanne</li><li>Fachexperten und Entwickler müssen während des Projektes täglich zusammenarbeiten</li><li>Errichte Projekte rund um motivierte Individuen. Gib ihnen das Umfeld und die Unterstützung, die sie benötigen und vertraue darauf, dass sie die Aufgabe erledigen</li><li>Die effizienteste und effektivste Methode, Informationen an und innerhalb eines Entwicklungsteam zu übermitteln, ist im Gespräch von Angesicht zu Angesicht</li><li>Funktionierende Software ist das wichtigste Fortschrittsmaß</li><li>Agile Prozesse fördern nachhaltige Entwicklung. Die Auftraggeber, Entwickler und Benutzer sollten ein gleichmäßiges Tempo auf unbegrenzte Zeit halten können</li><li>Ständiges Augenmerk auf technische Exzellenz und gutes Design fördert Agilität</li><li>Einfachheit — die Kunst, die Menge nicht getaner Arbeit zu maximieren — ist essenziell</li><li>Die besten Architekturen, Anforderungen und Entwürfe entstehen durch selbstorganisierte Teams</li><li>In regelmäßigen Abständen reflektiert das Team, wie es effektiver werden kann und passt sein Verhalten entsprechend an</li></ul><p>All das zusammen ist dem Agilen Manifest entnommen (siehe [MASD]).</p><p>Die Basis der Arbeit bilden grob definierte Ziele, die User Stories genannt werden. Gibt es mehrere zu einem ähnlichen Thema, so fasst man sie zu ""Epics"" zusammen. Eine User Story ist ähnlich aufgebaut, wie: ""Als Mitarbeiter in der Logistik möchte ich meine Waren digital erfassen können."" Ob hierzu ein User Interface eingebaut werden muss, oder eine Kamerasoftware, die einen Barcode einliest, ist hier noch nicht wichtig. Basierend auf [Coh04] schreibt A. Krallmann in [OS], über die Userstories:</p><ul><li>User-Stories sind dazu da, um Kommunikationen zwischen dem Fachbereich und Entwickler zu erzwingen; sie können für Iterationsplanungen verwendet werden und verschieben eine detailliertere Ausarbeitung auf später.</li><li>Akzeptanztests sind dazu da, um Details zu einer User-Story zu dokumentieren.</li><li>User-Stories sind transient und überleben nicht die Iteration, in der sie implementiert werden.</li><li>Die Nachteile von User-Stories sind insbesondere bei größeren Projekten deren Verwaltung, die Traceability zwischen ihnen und sie ersetzen keine Dokumentation des zu entwickelnden Systems.</li></ul><p>Trotzdem haben diese Userstories Vorteile.Gerade durch die Tatsache, das das Team darüber diskutiert, werden mehrere Sichtweisen berücksichtigt, die die Userstory bereichern.</p><p>Nach der gemeinsamen Formulierung werden diese dann im ""Product Backlog"" abgelegt.Dazu später mehr.</p><h3>Völlig von der Rolle</h3><p>Auch wenn gerade darüber diskutiert wird, weitere Rollen in das Konstrukt aufzunehmen, so gibt SCRUM aktuell drei Rollen vor:</p><ul><li>Product Owner</li><li>Entwicklungsteam</li><li>Scrum Master</li></ul><p>Auf Kundenseite gibt es:</p><ul><li>Stakeholders (Entscheidungsträger)</li><li>Users (Nutzer)</li></ul><h4>Product Owner</h4><p><figure><img class=""size-full"" src=""/img/blog/ScrumRoles2.jpg"" alt="""" width=""300"" height=""276"" /> <figcaption>Luis Reyes, CC-BY-NC-SA, blog.luis-reyes-plasencia.info</figcaption></figure></p><p>Der Product Owner ist für den wirtschaftlichen Erfolg verantwortlich und pflegt das ""Product Backlog"" zuständig.Dies ist ein Ort, an dem die Eigenschaften des Produktes(die künftigen) definiert werden.Mit dem Entwicklungsteam und den Stakeholdern werden hier neue Anforderungen eigepflegt. Der Product Owner ist derjenige, der regelmäßig mit den Stakeholdern spricht und versucht, Ihre Bedürfnisse zu verstehen. Auch ist er derjenige, der die Prioritäten der User Stories festlegen kann. Die ständige Priorisierung erfolgt meist aus einer Kosten-Nutzen-Relation.Das bedeutet, dass mit Scrum schnell vorzeigbare und nutzbare Ergebnisse vorliegen.</p><h4>Team</h4><p>Das Entwicklungsteam besteht aus drei bis neun Mitgliedern. Idealerweise hat jedes Mitglied andere Kenntnisse und Fähigkeiten, so das auch komplexere Projekte umgesetzt werden können. Das Team organisiert sich selbst, und lässt sich weder vom Scrum Master, noch vom Product Owner vorschreiben, wie Backlogeinträge umgesetzt werden. Die Schätzung des Umfangs einzelner Einträge obliegt ebenfalls dem Team. Vor einem neuen Sprint entnimmt das Team aus dem Product Backlog einige User Stories, und bricht sie auf einzelne Aufgaben (Tasks) herunter. Letztere sollten in spätestens einem Tag erledigt sein.All dies fließt in den Sprint Backlog ein.</p><p>Der Sprint Backlog ist derjenige, welches sich das Team selbst gibt. Es schätzt den Aufwand pro User Story, und übernimmt so viele Aufgaben, wie möglich in den Sprint.<br />Der Sprint selbst ist eine ständige Iteration.Nach dem Sprint ist vor dem Sprint.Er kann jeweils eine Woche oder auch einen Monat dauern. In jedem Fall ist es wichtig, am Ende ein funktionales Produkt auszuliefern.</p><h4>Scrum Master</h4><p>Jetzt fehlt zuletzt noch die dritte Rolle: der ""Scrum Master"". Er kann Bestandteil des Teams sein, muss er aber nicht. So wirkt er als Coach und verantwortlich für das Gelingen des Teams. Zudem räumt alle Hindernisse aus dem Weg, wie möglicherweise mangelnde Kommunikation, persönlichen Spannungen im Team oder Störungen in der Zusammenarbeit mit dem Product Owner. Diese Rolle erfordert Fähigkeiten im situativen Führen (siehe zB Ken Blanchard).</p><h3>Der Sprint</h3><p>Wie ist der Ablauf eines Sprints?<br /> Zu Beginn steht die Planungsphase.Es wird festgelegt, was entwickelt wird, und ebenfalls wie es entwickelt wird. Was entwickelt werden soll, wird vom Product Owner festgelegt.</p><h4>Definition of Done</h4><p>Das Team erarbeitet gemeinsame Akzeptanzkriterien (Definition of Done). Somit wird auch die Frage danach beantwortet, wann eine Arbeit als fertig zu gelten hat.Anfangs benötigt dieser Schritt vielleicht etwas mehr Zeit.Aber durch weitere Iterationen und wachsender Erfahrung werden auch hier Grundlagen gelegt, die für weitere Iterationen gelten können. Grundsätzlich gilt es auch in einer Scrum-Umgebung reguläre Tests, sowie eine Entwicklerdokumentation anzufertigen. Auch diese Positionen kosten Zeit und müssen implementiert werden.Schlußendlich jedoch spart es Zeit, da diese Schritte zur Gewohnheit werden, und der Qualitätsstandard gehalten, oft noch weiter übertroffen werden kann.</p><p>Wie bereits erwähnt, kann (und sollte) der Product Owner die Priorisierung festlegen.Dennoch obliegt es dem Team zu entscheiden, was im folgenden Sprint umgesetzt wird. Gemeinsam formuliert das Team ein Ziel des Sprints, einen Forecast (wahrscheinliches Sprintende).<br />Im zweiten Teil des Sprints wird vom Team das Wie besprochen.Dies alles fließt nun in den Sprint-Backlog ein.</p><p>Das Scrum Manifest macht zur Umsetzung des Backlogs keine Festlegungen, was sowohl ein einfaches unterteiltes Whiteboard sein kann, sowie ein professionelles Computerprogramm.</p><p> <figure><img class=""size-full"" src=""/img/blog/ScrumBoard.png"" alt="""" width=""40%"" /><figcaption>Author: Wikimedia Commons contributors, Publisher: Wikimedia Commons, the free media repository, Permanent URL: https://commons.wikimedia.org/w/index.php?title=File:ScrumBoard_(png).png&amp;oldid=219089599</figcaption></figure></p><p> In meinem letzten Unternehmen im Fintech Bereich hatten wir ein großes Board, unterteilt in: ToDo, In Progress, To Verify und Done.Anhand der Begriffe lässt sich die deutsche Bedeutung ableiten.Sinn dieser sehr offenen Art und Weise ist es, Transparenz zu leben.Jeder kann einsehen, wie weit das Team gekommen ist und ob und wo es hakt.Hier kann dann frühzeitig dagegen angegangen werden.</p><p>Der Daily Scrum ist ein tägliches, etwa 15 Minütiges Treffen des Teams, in dem besprochen Wird, wie der aktuelle Status ist.Falls dabei festgestellt wird, dass eine Aufgabe länger als geschätzt dauert, wird sie aufgeteilt.Größere Probleme, die die Meetingzeit überschreiten, werden dem Scrum Master übermittelt.</p><p><figure><img class=""alignright size-large"" src=""/img/blog/scrum_visualized.png"" alt="""" width=""40%"" /><figcaption>Author: Gregory Heller</figcaption></figure></p><p>Ist der Sprint beendet, beginnt der Sprint Review. Hier findet die Präsentation bei Kunden statt.Eingehendes Feedback wird vom Product Owner aufgenommen und für die weitere Gestaltung des Product Backlogs(möglicherweise auch zur Priorisierung) verwendet.Der Sprint Review dauert ca. 1 Stunde pro Sprint.Durch ständige Iterationen wird sowohl das eigentliche ""Fertigwerden"", als auch die Produktqualität zur Routine.</p><p>Mit der wichtigste Punkt für das Team ist die Retrospektive. Hier wird ehrlich die Arbeitsweise überprüft, und Möglichkeiten gefunden, sich besser, effizienter und effektiver zu organisieren.Um die Offenheit zu fördern, findet dieses Meeting nur mit dem Team und dem Scrum Master statt. Insgesamt ist die Retrospektive aber ein Teil, der dem Team beim wachsen hilft. Jedes Individuum lernt für sich, und auch die Gruppe lernt besser zusammenzuarbeiten.</p><p>Der letzte Punkt des Sprints ist zugleich der Beginn des neuen: Product Backlog Refinement.Hier werden Einträge geordnet, Einträge gelöscht, neue Einträge hinzugefügt, Einträge werden Detailiert oder zusammengefasst, Userstories werden geschätzt und Releases werden geplant.</p><p>Somit wurde ein kompletter Schaffenszyklus eingeführt, verbessert, abgenommen und abgeschlossen.</p> <p>Auf manche Punkte wie zB das ""Planning Poker"" zur Aufwandschätzung und ein paar andere bin ich an dieser Stelle nicht weiter eingegangen, das dieser Post nur einen Einstieg in die Welt von Scrum geben sollte.</p><p>Quellen:<ul><li>[WP] Seite „Scrum“. In: Wikipedia, Die freie Enzyklopädie.Bearbeitungsstand: 30. Juni 2018, 09:54 UTC.URL: <a href = ""https://de.wikipia.org/w/index.php?title=Scrum&amp;oldid=178749076"" > https://de.wikipedia.org/w/index.php?title=Scrum&amp;oldid=178749076</a> (Abgerufen: 5. Juli 2018, 19:02 UTC)</li><li>[MASD] Manifesto for Agile Software Development, URL: <a href = ""http://agilemanifesto.org/"" > http://agilemanifesto.org/</a> (Abgerufen: 5. Juli 2018, 19:03 UTC)</li><li>[Coh04] M.Cohn, User Stories Applied, Addison-Wesley, 2004</li><li>[OS] A.Krallmann, Modellbasiertes Requirements Engineering, Objekt Spektrum 04/2018, S. 51</li></ul>"
      },
      new BlogStory()
      {
        Id = 8,
        IsPublished = true,
        Title = "Neuer ASP.NET Core Blog",
        Slug = "2018/11/18/Neuer-ASP-NET-Core-Blog",
        Categories = "Development, DotNetCore, ASP.NET Core, de-DE",
        DatePublished = new DateTime(2018, 11, 18),
        Body = @"<p>Seit Ende der 90er Jahre des vergangenen Jahrhunderts bin ich schreibend im Internet unterwegs. Die ersten Webseiten habe ich in rudimentärem HTML geschrieben und via FTP bei Compuserve hochgeladen . Auch wenn Tim Berners-Lee’s Kollege <a href=""https://de.wikipedia.org/wiki/H%C3%A5kon_Wium_Lie"" target=""_blank"">Håkon Wium Lie</a> 1994 am CERN den ersten Entwurf für das Cascading Stylesheet entwarf, dauerte es etwas, bis es flächendeckend benutzt wurde. So bin ich selbst erst in den 2000er Jahren dazu gekommen.</p><p>Meine Reise ging über viele Stationen: Blogger, Wordpress (Selbst- und Fremdgehostet), einer selbstgehosteten Joomla-Plattform und Github Pages. Letzteres fand ich interessant, da man aus reinem Markdown-Code statische Webseiten generieren konnte. Dennoch fehlte etwas der dynamische Teil. Ebenfalls musste für jeden Blogbeitrag ein Commit erstellt werden.</p><p>In den letzten Monaten formte sich in mir der Wunsch einen Blog zu nutzen, dessen komplette Codebase mir bekannt ist. Es sollte eine Codebasis sein, die möglichst flexibel und erweiterbar sein sollte, dabei jedoch so wenig aufgebläht sein sollte, wie bisher getestete CMS.</p><p>So stolperte ich über der <a href=""https://github.com/shawnMannsmuth/MannsBlog"" target=""_blank"">MannsBlog</a>, der bereits viele Features bot, die ich suchte. Er basierte auf dem ASP.NET Core, dem Entity Framework 2.0, MVC6 und Bootstrap 4. Zusätzlich bot es externe Dienste wie SendGrid (via VueJS) und Disqus (via JavaScript) an. Als UI zum Verfassen der Blogposts kommt der <a href=""https://openlivewriter.org/"" target=""_blank"">Open LiveWriter</a> zum Einsatz.</p><p>Ich erstellte einen Fork dieses Blogs. Die Grundimplementierung ist aktuell noch nahezu die gleiche, wie im MannsBlog. Lediglich die Podcast Web-API wurde entfernt (samt Abhängigkeiten), und das Blog DSVGO-Konform gemacht. </p><p>Da ich öfters Inhalte zweisprachig veröffentliche plane ich künftig die “Cultures”-Implementierung von ASP.NET Core zur Ermöglichung von Globalisierung und Lokalisierung. Grundsätzlich schickt jeder Webseitenbesucher seine Sprache und sein Land bei jeder Browseranfrage mit. Dies wäre einfach auszuwerten. Dann lediglich das Routing anpassen und Ressourcendateien schreiben und schon haben wir einen mehrsprachigen Blog. </p><p>Daher wird in künftigen Versionen des Blogs auch ein erweiterter Quellcode verfügbar sein.</p><p>Getreu den Idealen des MannsBlogs stelle ich auch den Code dieser Webseite als OpenSource zur Verfügung. Das Repository liegt auf Azure DevOps unter: <a title=""https://github.com/saigkill/saschamannsde"" href=""https://github.com/saigkill/saschamannsde"" target=""_blank"">https://github.com/saigkill/saschamannsde</a> .</p>"
      },
      new BlogStory()
      {
        Id = 9,
        IsPublished = true,
        Title = "Windows Insider Log 0 (de)",
        Slug = "2018/11/24/Windows-Insider-Log-0-(de)",
        Categories = "Windows,Insider,de-DE",
        DatePublished = new DateTime(2018, 11, 24),
        Body = @"<p>Seit einigen Monaten bin ich als Windows Insider tätig, und versuche durch Bugreports und Feature Requests die Benutzererfahrung zu verbessern. <p>Ein zentrales Tool zur Kommunikation ist der in Windows 10 eingeführte “Feeback Hub”. Hier können eigene Wünsche oder gefundene Fehler eingereicht werden. Jeder Windows 10 Anwender hat die Möglichkeit, seinen Feeback Hub zu öffnen, und das eingereichte Feedback zu bewerten.</p><p>Öffnen Sie dazu einfach den Feedback Hub und öffnen Sie ein Feedback. Möchten Sie sich dem Feeback anschließen, klicken Sie einfach auf das Wort “Stimme”. Die Anzahl der Stimmen wird augenblicklich um 1 erhöht. Möchten Sie einen Kommentar scheiben, so können Sie es im unteren Bereich des Feedbacks tun.</p><p>Alle Feedbacks werden von Microsoft gesichtet. In vielen Fällen steht dann unter dem Feedback, das Feedback sei angekommen oder bereits in Bearbeitung.</p><p>Jetzt stelle ich Ihnen mein eingereichtes Feedback vor. Falls Sie das Feedback ebenfalls für wichtig halten, so bitte ich um Ihre Stimme.</p><h3>Dotfiles zu Ausgeblendete Elemente hinzufügen</h3><p>Es gibt Programme, die sowohl unter Windows, als auch unter Linux funktionieren. Manche legen Config-Dateien und versteckte Verzeichnisse im Linux-Style an ($HOME/.Verzeichnis). Ich würde vorschlagen diese Dateien und Verzeichnisse, die mit einem Punkt beginnen automatisch auszublenden.</p><p>Link zu Feedback Hub: <a title=""https://aka.ms/AA39m0d"" href=""https://aka.ms/AA39m0d"">https://aka.ms/AA39m0d</a></p><h3>Visueller XAML-Designer für Xamarin.Forms</h3><p>Ich würde gerne vorschlagen, Visual Studio um einen visuellen XAML-Designer für Xamarin.Forms zu ergänzen.</p><p>Link zu Feedback Hub: <a title=""https://aka.ms/AA3acyy"" href=""https://aka.ms/AA3acyy"">https://aka.ms/AA3acyy</a></p><h3>Unterstützung des HTML Tags ""Details""</h3><p>Die meisten Browser unterstützen schon länger das HTML-Tag ""Details"". Gerne hätte ich diese Tag-Unterstützung in Edge.</p><p>Link zu Feedback Hub: <a title=""https://aka.ms/AA3acz0"" href=""https://aka.ms/AA3acz0"">https://aka.ms/AA3acz0</a></p><h3>Fehlercode 7017 bei Amazon Prime</h3><p>Seit dem Build 18282.1000 habe ich das Problem, das bei jedem beliebigen Film in Amazon Prime, nach einer gewissen Ladezeit der Fehler 7017 auftaucht. Der Fehler ist reproduzierbar, tritt aber weder beim Firefox noch beim Google Chrome auf.</p><p>Link zu Feedback Hub: <a title=""https://aka.ms/AA3acz1"" href=""https://aka.ms/AA3acz1"">https://aka.ms/AA3acz1</a></p>"
      },
      new BlogStory()
      {
          Id = 10,
          IsPublished = true,
          Title = "Adding a estimated time to read to a .NET Core Blog with Razor",
          Slug = "2019/04/04/Adding-a-estimated-time-to-read-to-a-NET-Core-Blog-with-Razor",
          Categories = "Development,DotNetCore,en-US",
          DatePublished = new DateTime(2019,4,14),
          Body = @"<p>This week i implemented a feature what shows you, how many time you have to spend, to read a blog article.</p> <p>In the first step, i used my model BlogStory to get the content. Then Razor does the following:</p> <p> It counts the spaces between the words and adds 1. So now we knowing how many words we have in that article. The most people can read 200 to 250 in one minute. So we need to divide the counted words with 250. Then we knowing, how many minutes it takes to read. Then we combine a modulo with a divide to get the seconds. </p> <pre><code> @{ var word_count = @Model.Body;<br /> var counts = word_count.Count(ch => ch == ' ') + 1;<br> var minutes = counts / 250; var seconds = counts % 250 / (250 / 60);<br /> var str_minutes = (minutes == 1) ? ""Minute"" : ""Minutes"";<br/> var str_seconds = (seconds == 1) ? ""Second"" : ""Seconds"";<br>} </pre></code> <p>Now we placing the code for displaying the estimated time to read</p> <pre><code> <i class=""fas fa-clock""></i> @minutes @str_minutes @seconds @str_seconds </pre></code> <p>On the place, where this snipped is, will be the estimated time to read.</p> <p>Currently it is placed on client side. It is planned to move that function to server side in future.</p>"
      },
      new BlogStory()
      {
          Id = 11,
          IsPublished = true,
          Title = "Animated Typing Utility with JavaScript",
          Slug = "2019/05/21/Animated-Typing-Utility-with-JavaScript",
          Categories = "Development,JavaScript,en-US",
          DatePublished = new DateTime(2019,5,21),
          Body = @"<p>Ever wished a animated typing effect in your webproject? If yes, you should definitive check out the typit.js library. Last days i stumbled upon that interesting library.</p><p>The effect looks like:</p><p><img src=""https://raw.githubusercontent.com/alexmacarthur/typeit/master/readme-demo.gif"" alt="""" style=""border:5px solid #24292e""></p><p>How could you implement it?</p><p>Let we say, you would like to use this effect to show others your different job experiences.</p><p>First place a tag inside your code like that one:<p id=""replaceJobs""></p> Then create a typeit.js file with the content</p><p><pre><code>new TypeIt('#replaceJobs', { strings: [""Job1"", ""Job2"", ""Job3""], speed: 200, cursorSpeed: 1000, nextStringDelay: 750, loop: true, breakLines: false, waitUntilVisible: true }).go(); Now add the JavaScript to your Layout Section:</p><p><script src=""https://cdn.jsdelivr.net/npm/typeit@VERSION_NUMBER/dist/typeit.min.js""></script> and refer your typeit.js to that page, where your ""replaceJobs"" tag is placed. Now your type effect will start :-)</p><p>More on: <a href=""https://typeitjs.com/"">https://typeitjs.com/</a></p>"
      },
      new BlogStory()
      {
          Id = 12,
          IsPublished = true,
          Title = "Latex Curriculum Vitae 0.9.0 released!",
          Slug = "2020/10/22/Latex-Curriculum-Vitae-0.9.0-released",
          Categories = "Development,DotNetCore,C-Sharp,en-US",
          DatePublished = new DateTime(2020,10,22),
          Body = @"<p>After 4 weeks of hard work i'm pleased to announce the first public beta of Latex Curriculum Vitae.</p> <p>What is Latex Curriculum Vitae? It is a little helper for writing job applications. I'm deliver a set of LaTEX documents where you can modify, or just use your already created one.</p> <p>Inside Latex Curriculum Vitae you just have to type the job application dependend information. Latex Curriculum Vitae builds the LaTEX code, creates PDFs and sends out to the company.</p> <p><img src=""https://dev-to-uploads.s3.amazonaws.com/i/s0fr9mjii77hcx29k6wl.png"" width=""90%"" alt=""Alt Text""></p> <p>Automatically it creates a CSV file, what you can use for printing all of your application details. Also it has a database, where you can manage the current status of your applications.</p> <p>More information on: (<a href=""https://saigkill.github.io/latex_curriculum_vitae-dotnet/"">https://saigkill.github.io/latex_curriculum_vitae-dotnet/</a>).</p> <p>If you are an coder so please read: (<a href=""https://github.com/saigkill/latex_curriculum_vitae-dotnet/blob/master/CONTRIBUTING.md"">https://github.com/saigkill/latex_curriculum_vitae-dotnet/blob/master/CONTRIBUTING.md</a>). I need currently some help in some cases.</p> <p>The last Exe can downloaded on (<a href=""https://github.com/saigkill/latex_curriculum_vitae-dotnet/releases"">https://github.com/saigkill/latex_curriculum_vitae-dotnet/releases</a>).</p>"
      }
    };
    }
}
